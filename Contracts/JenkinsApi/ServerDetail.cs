using System.Collections.Generic;

namespace Contracts.JenkinsApi
{
    public class ServerDetail
    {
        public string Mode { get; set; }
        public string NodeDescription { get; set; }
        public string NodeName { get; set; }
        public int NumExecutors { get; set; }
        public string Description { get; set; }
        public List<Job> Jobs { get; set; }
        public View PrimaryView { get; set; }
        public bool QuietingDown { get; set; }
        public int SlaveAgentPort { get; set; }
        public bool UseCrumbs { get; set; }
        public bool UseSecurity { get; set; }
        public List<View> Views { get; set; }
    }
}