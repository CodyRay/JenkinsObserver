using System.Collections.Generic;

namespace Contracts.JenkinsApi
{
    public class BuildDetail
    {
        public List<Action> Actions { get; set; }
        public List<object> Artifacts { get; set; }
        public bool Building { get; set; }
        public int Duration { get; set; }
        public int EstimatedDuration { get; set; }
        public string FullDisplayName { get; set; }
        public string Id { get; set; }
        public bool KeepLog { get; set; }
        public int Number { get; set; }
        public string Result { get; set; }
        public long Timestamp { get; set; }
        public string Url { get; set; }
        public string BuiltOn { get; set; }
        public ChangeSet ChangeSet { get; set; }
        public List<object> Culprits { get; set; }
        public List<Run> Runs { get; set; }
    }
}