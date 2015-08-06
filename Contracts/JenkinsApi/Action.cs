using System.Collections.Generic;

namespace Contracts.JenkinsApi
{
    public class Action
    {
        public List<Cause> Causes { get; set; }
        public Revision LastBuiltRevision { get; set; }
        public List<string> RemoteUrls { get; set; }
        public string ScmName { get; set; }
    }
}