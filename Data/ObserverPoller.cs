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
    public delegate void JobChangeEvent(ObserverPoller sender, ObserverServer server, ObserverJob job, ChangeType change);

    public class ObserverPoller
    {
        protected SettingsStorage Data { get; set; }

        static ObserverPoller()
        {
            Mapper.CreateMap<JobDetail, ObserverJob>()
                .ForMember(j => j.Enabled, e => e.Ignore())
                .ForMember(j => j.Status, e => e.ResolveUsing(d => ObserverJob.GetJobsStatus(d.Color).Item1))
                .ForMember(j => j.InProgress, e => e.ResolveUsing(d => ObserverJob.GetJobsStatus(d.Color).Item2));
            Mapper.CreateMap<ServerDetail, ObserverServer>()
                .ForMember(s => s.Jobs, e => e.Ignore())
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

        private void SendStatusChanged(ObserverServer server, ObserverJob job, ChangeType change)
        {
            JobChanged?.Invoke(this, server, job, change);
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
#if DEBUG
                        await Task.Delay((TimeSpan.FromSeconds(settings.PollingPeriod)), token); //For Debug a minute is a sec
#else
                        await Task.Delay(TimeSpan.FromMinutes(settings.PollingPeriod), token);
#endif
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
            var serverDetail = await Request<ServerDetail>(server.Url, token);
            if (serverDetail == null)
            {
                SendStatusChanged(server, null, ChangeType.ErrorPollingServer);
                return;
            }

            Mapper.Map(serverDetail, server); //The Jobs will be handled manually below

            #region Add New Jobs

            foreach (var j in Clone(serverDetail.Jobs).Where(j => server.Jobs.All(job => job.Name != j.Name)))
            {
                var jDetail = await Request<JobDetail>(j.Url, token);
                if (jDetail == null)
                {
                    serverDetail.Jobs.RemoveAll(job => job.Name == j.Name);
                    continue; //Note: I am unsure how this could ever happen
                }

                var jObserver = Mapper.Map<JobDetail, ObserverJob>(jDetail);
                jObserver.Enabled = true; //By default assume that the user want so see notificications
                server.Jobs.Add(jObserver);
                SendStatusChanged(server, jObserver, ChangeType.NewJobFound);
            }

            #endregion

            #region Remove Missing Jobs

            //Clone to prevent enumeration exception from removing element while enumerating
            foreach (var j in Clone(server.Jobs).Where(j => serverDetail.Jobs.All(job => job.Name != j.Name)))
            {
                server.Jobs.Remove(server.Jobs.Single(job => job.Name == j.Name));
                SendStatusChanged(server, j, ChangeType.MissingJob);
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

                var job = await Request<JobDetail>(newDetailJob.Url, token);
                if (job != null)
                {
                    Mapper.Map(job, observerJob);
                    SendStatusChanged(server, observerJob, changeType.Value);
                }
                else
                {
                    SendStatusChanged(server, observerJob, ChangeType.ErrorPollingJob);
                }
            }
        }

        #region Methods

        protected static T Clone<T>(T server)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(server));
        }

        private async Task<T> Request<T>(string url, CancellationToken token) where T : class
        {
            try
            {
                var uri = new Uri(new Uri(url), "api/json");
                using (var client = new HttpClient())
                {
                    var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri), token);
                    var text = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(text);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion Methods
    }
}
 