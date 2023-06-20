using System;

namespace CascadePass.TrailBot
{
    public class ProviderStatistics
    {
        public ProviderStatistics()
        {
            this.SleepTime = new();
        }

        public int RequestsMade { get; set; }

        public int FailedRequests { get; set; }

        public int MatchesFound { get; set; }

        public TimeSpan SleepTime { get; set; }
    }
}
