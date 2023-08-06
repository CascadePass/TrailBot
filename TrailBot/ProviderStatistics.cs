using System;

namespace CascadePass.TrailBot
{
    /// <summary>
    /// Tracks aggregate statistics about a <see cref="WebDataProvider"/>'s
    /// activities.
    /// </summary>
    public class ProviderStatistics
    {
        /// <summary>
        /// Gets or sets the total number of requests the <see cref="WebDataProvider"/>
        /// has made, regardless of outcome.
        /// </summary>
        public int RequestsMade { get; set; }

        /// <summary>
        /// Gets or sets the number of requests that have failed, for any reason.
        /// </summary>
        public int FailedRequests { get; set; }

        /// <summary>
        /// Gets or sets the number of matches the <see cref="WebDataProvider"/> made.
        /// </summary>
        public int MatchesFound { get; set; }

        /// <summary>
        /// Gets or sets the total, cumulative amount of time the <see cref="WebDataProvider"/>
        /// has spent sleeping between requests
        /// </summary>
        public TimeSpan SleepTime { get; set; }
    }
}
