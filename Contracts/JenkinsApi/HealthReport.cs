namespace Contracts.JenkinsApi
{
    public class HealthReport
    {
        public string Description { get; set; }
        public string IconClassName { get; set; }
        public string IconUrl { get; set; }
        public int Score { get; set; }
    }
}