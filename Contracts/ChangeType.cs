namespace Contracts
{
    public enum ChangeType
    {
        BuildCompleted,
        BuildStarted,
        BuildStatusChange,
        MissingJob,
        NewJobFound,
        ErrorPollingServer,
        ErrorPollingJob
    }
}