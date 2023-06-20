using System.Collections.Generic;

namespace CascadePass.TrailBot
{
    public class MatchInfo
    {
        public MatchInfo()
        {
            this.MatchCounts = new();
            this.MatchQuotes = new();
            this.Exerpts = new();
        }

        public Topic Topic { get; set; }

        public bool IsEmpty => this.MatchCounts.Count == 0;

        public int Count => this.MatchCounts.Count;

        public int WordCount { get; set; }

        public Dictionary<string, int> MatchCounts { get; set; }

        public List<string> MatchQuotes { get; set; }

        public List<Exerpt> Exerpts { get; set; }
    }
}
