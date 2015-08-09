using AutoMapper;
using Contracts;
using Contracts.JenkinsApi;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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
                .ForMember(j => j.Status, e => e.Ignore())
                .ForMember(j => j.InProgress, e => e.Ignore());
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
            if (JobChanged != null)
            {
                JobChanged(this, job, change);
            }
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
            var serverOriginal = Clone(server);
            var serverDetail = await GetServerDetailAsync(server.Url, token);
            Mapper.Map(serverDetail, server);
            server.Jobs.Clear();
            foreach (var j in serverDetail.Jobs)
            {
                var jDetail = await GetJobDetailAsync(j.Url, token);
                var jObserver = Mapper.Map<JobDetail, ObserverJob>(jDetail);
                server.Jobs.Add(jObserver);
            }
            CheckServerForChange(serverOriginal, server);
        }

        protected void CheckServerForChange(ObserverServer beforePoll, ObserverServer afterPoll)
        {
            //TODO: Overload Equals in Job, then get rid of all this weirdness
            var beforeJobs = beforePoll.Jobs.ToDictionary(j => j.Name);
            var afterJobs = afterPoll.Jobs.ToDictionary(j => j.Name);
            foreach (var jName in afterJobs.Keys)
            {
                if (beforeJobs.ContainsKey(jName))
                    CheckJobForChange(beforeJobs[jName], afterJobs[jName]);
                else
                    SendStatusChanged(afterJobs[jName], ChangeType.NewJobFound);
            }
            foreach (var jName in beforeJobs.Keys.Where(j => !afterJobs.ContainsKey(j)))
            {
                SendStatusChanged(beforeJobs[jName], ChangeType.MissingJob);
            }
        }

        protected void CheckJobForChange(ObserverJob oldJob, ObserverJob newJob)
        {
            //ChangeType.BuildCompleted;
            if (oldJob.InProgress && !newJob.InProgress)
                SendStatusChanged(oldJob, ChangeType.BuildCompleted);
            //ChangeType.BuildStarted;
            if (oldJob.InProgress && !newJob.InProgress)
                SendStatusChanged(oldJob, ChangeType.BuildStarted);
            //ChangeType.BuildStatusChange;
            if (oldJob.WebColor != newJob.WebColor)
                //Could use status, but this will exclude changes between the same color for us
                SendStatusChanged(oldJob, ChangeType.BuildStatusChange);
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