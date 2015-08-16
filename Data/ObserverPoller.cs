using AutoMapper;
using Contracts;
using Contracts.JenkinsApi;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Data
{
    public delegate void JobChangeEvent(ObserverPoller sender, ObserverJob job, ChangeType change);

    public class ObserverPoller
    {
        protected SettingsStorage Data { get; set; }

        static ObserverPoller()
        {
            Mapper.CreateMap<JobDetail, ObserverJob>()
                .ForMember(j => j.Id, e => e.Ignore())
                .ForMember(j => j.Enabled, e => e.Ignore())
                .ForMember(j => j.Status, e => e.ResolveUsing(d => ObserverJob.GetJobsStatus(d.Color).Item1))
                .ForMember(j => j.InProgress, e => e.ResolveUsing(d => ObserverJob.GetJobsStatus(d.Color).Item2));
            Mapper.CreateMap<ServerDetail, ObserverServer>()
                .ForMember(s => s.Jobs, e => e.Ignore())
                .ForMember(s => s.Id, e => e.Ignore())
                .ForMember(s => s.Enabled, e => e.Ignore())
                .ForMember(s => s.Name, e => e.Ignore())
                .ForMember(s => s.Url, e => e.ResolveUsing(s => s.PrimaryView.Url));
            Mapper.AssertConfigurationIsValid();
        }

        public ObserverPoller()
        {
            Data = new SettingsStorage();
        }

        public event JobChangeEvent JobChanged;

        private void SendStatusChanged(ObserverJob job, ChangeType change)
        {
            JobChanged?.Invoke(this, job, change);
        }

        public async Task Run(CancellationToken token)
        {
            try
            {
                do
                {
                    var settings = Data.Settings;
                    try
                    {
                        foreach (var server in settings.Servers)
                        {
                            await PollServer(server, token);
                        }
                        await Task.Delay(settings.PollingPeriod, token);
                    }
                    finally
                    {
                        Data.Settings = settings;
                    }
                } while (!token.IsCancellationRequested);
            }
            catch (TaskCanceledException)
            {
                //Ignored
            }
        }

        public async Task PollServer(ObserverServer server, CancellationToken token)
        {
            var serverDetail = await GetServerDetailAsync(server.Url, token);
            Mapper.Map(serverDetail, server); //The Jobs will be handled manually below

            #region Add New Jobs

            foreach (var j in serverDetail.Jobs.Where(j => server.Jobs.All(job => job.Name != j.Name)))
            {
                var jDetail = await GetJobDetailAsync(j.Url, token);
                var jObserver = Mapper.Map<JobDetail, ObserverJob>(jDetail);
                server.Jobs.Add(jObserver);
                SendStatusChanged(jObserver, ChangeType.NewJobFound);
            }

            #endregion

            #region Remove Missing Jobs

            //Clone to prevent enumeration exception from removing element while enumerating
            foreach (var j in Clone(server.Jobs).Where(j => serverDetail.Jobs.All(job => job.Name != j.Name)))
            {
                server.Jobs.Remove(server.Jobs.Single(job => job.Name == j.Name));
                SendStatusChanged(j, ChangeType.MissingJob);
            }

            #endregion

            //Now that they have the same number of elements, we can update ones that have their status changed
#if DEBUG
            Debug.Assert(server.Jobs.Count == serverDetail.Jobs.Count);
#endif
            foreach (var newDetailJob in serverDetail.Jobs)
            {

                var observerJob = server.Jobs.Single(j => j.Name == newDetailJob.Name);
                var statusTuple = ObserverJob.GetJobsStatus(newDetailJob.Color);
                var newStatus = statusTuple.Item1;
                var newInProgress = statusTuple.Item2;

                /*
                 * Note that only one event will be fired for any of these, this greatly simplifies 
                 * the behavior of this code. The downside is that BuildStatusChange is given the  
                 * highest priority. i.e. if a build finishes AND the status changes only a 
                 * BuildStatusChange will be fired
                 */

                ChangeType? changeType = null;
                //ChangeType.BuildCompleted;
                if (observerJob.InProgress && !newInProgress)
                    changeType = ChangeType.BuildCompleted;

                //ChangeType.BuildStarted;
                if (!observerJob.InProgress && newInProgress)
                    changeType = ChangeType.BuildStarted;

                //ChangeType.BuildStatusChange;
                if (observerJob.Status != newStatus)
                    changeType = ChangeType.BuildStatusChange;

                if (!changeType.HasValue)
                    continue; //No need to poll the details of a job that hasn't changed

                var job = await GetJobDetailAsync(newDetailJob.Url, token);
                Mapper.Map(job, observerJob);
                SendStatusChanged(observerJob, changeType.Value);
            }
        }

        #region Methods

        protected static T Clone<T>(T server)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(server));
        }

        protected Task<JobDetail> GetJobDetailAsync(string url, CancellationToken token)
        {
            var uri = new Uri(new Uri(url), "api/json");
            return Request<JobDetail>(uri, token);
        }

        private async Task<T> Request<T>(Uri uri, CancellationToken token) where T : class
        {
            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri), token);
                var text = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(text);
            }
        }

        protected Task<ServerDetail> GetServerDetailAsync(string url, CancellationToken token)
        {
            var uri = new Uri(new Uri(url), "api/json");
            return Request<ServerDetail>(uri, token);
        }

        #endregion Methods
    }
}
 