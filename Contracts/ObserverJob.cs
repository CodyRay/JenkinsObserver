using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;

namespace Contracts
{
    [ImplementPropertyChanged]
    public class ObserverJob
    {
        [Key]
        public long Id { get; set; }

        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public JobStatus Status { get; set; }
        public bool InProgress { get; set; }

        [DependsOn("Status")]
        public string WebColor
        {
            get
            {
                switch (Status)
                {
                    case JobStatus.Unknown:
                        return "BLUE";

                    case JobStatus.Aborted:
                    case JobStatus.Disabled:
                    case JobStatus.NotBuilt:
                    case JobStatus.Pending:
                        return "GREY";

                    case JobStatus.Failed:
                        return "RED";

                    case JobStatus.Unstable:
                        return "YELLOW";

                    case JobStatus.Success:
                        return "GREEN";

                    default:
                        throw new InvalidOperationException("Bad Status");
                }
            }
        }

        public static Tuple<JobStatus, bool> GetJobsStatus(string ball_color)
        {
            JobStatus status;
            bool inProgress;
            switch (ball_color.ToUpper())
            {
                case "ABORTED":
                case "ABORTED_ANIME":
                    status = JobStatus.Aborted;
                    break;

                case "BLUE":
                case "BLUE_ANIME":
                    status = JobStatus.Success;
                    break;

                case "DISABLED":
                case "DISABLED_ANIME":
                    status = JobStatus.Disabled;
                    break;

                case "GREY":
                case "GREY_ANIME":
                    status = JobStatus.Pending;
                    break;

                case "NOTBUILT":
                case "NOTBUILT_ANIME":
                    status = JobStatus.NotBuilt;
                    break;

                case "RED":
                case "RED_ANIME":
                    status = JobStatus.Failed;
                    break;

                case "YELLOW":
                case "YELLOW_ANIME":
                    status = JobStatus.Unstable;
                    break;

                default:
                    throw new InvalidOperationException("Unexpected Ball_Color " + ball_color);
            }

            switch (ball_color.ToUpper())
            {
                case "ABORTED":
                case "BLUE":
                case "DISABLED":
                case "GREY":
                case "NOTBUILT":
                case "RED":
                case "YELLOW":
                    inProgress = false;
                    break;

                case "ABORTED_ANIME":
                case "BLUE_ANIME":
                case "DISABLED_ANIME":
                case "GREY_ANIME":
                case "NOTBUILT_ANIME":
                case "RED_ANIME":
                case "YELLOW_ANIME":
                    inProgress = true;
                    break;

                default:
                    throw new InvalidOperationException("Unexpected Ball_Color " + ball_color);
            }

            return Tuple.Create(status, inProgress);
        }
    }
}