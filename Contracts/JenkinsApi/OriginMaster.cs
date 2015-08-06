namespace Contracts.JenkinsApi
{
    public class OriginMaster
    {
        public int BuildNumber { get; set; }
        public Marked Marked { get; set; }
        public Revision Revision { get; set; }
    }
}