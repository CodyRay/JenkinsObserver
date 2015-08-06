using System.Collections.Generic;

namespace Contracts.JenkinsApi
{
    public class ActiveConfigurationDetail
    {
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string DisplayNameOrNull { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Buildable { get; set; }
        public List<Build> Builds { get; set; }
        public string Color { get; set; }
        public Build FirstBuild { get; set; }
        public List<HealthReport> HealthReport { get; set; }
        public bool InQueue { get; set; }
        public bool KeepDependencies { get; set; }
        public Build LastBuild { get; set; }
        public Build LastCompletedBuild { get; set; }
        public Build LastFailedBuild { get; set; }
        public Build LastStableBuild { get; set; }
        public Build LastSuccessfulBuild { get; set; }
        public Build LastUnstableBuild { get; set; }
        public Build LastUnsuccessfulBuild { get; set; }
        public int NextBuildNumber { get; set; }
        public bool ConcurrentBuild { get; set; }
    }
}