using System;

namespace CascadePass.TrailBot.DataAccess.DTO
{
    public class ProviderStatistics : DataTransferObject
    {
        public long ID { get; set; }

        public long MatchesFound { get; set; }

        public long TotalRequestsMade { get; set; }

        public long FailedRequests { get; set; }

        public long SleepTimeInMS { get; set; }

        public TimeSpan SleepTime
        {
            get => TimeSpan.FromMilliseconds(this.SleepTimeInMS);
            set
            {
                this.SleepTimeInMS = (long)value.TotalMilliseconds;
            }
        }
    }
}
