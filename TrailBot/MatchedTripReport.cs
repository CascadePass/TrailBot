using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CascadePass.TrailBot
{
    public class MatchedTripReport
    {
        public MatchedTripReport()
        {
            this.Topics = new();
        }

        #region Properties

        #region Instance

        public string SourceUri { get; set; }

        public DateTime TripDate { get; set; }

        public string Title { get; set; }

        public string HikeType { get; set; }

        public string Region { get; set; }

        //[XmlIgnore]
        //public TripReport Report { get; set; }

        public string Matches { get; set; }

        public string Filename { get; set; }

        public int WordCount { get; set; }

        public int MatchingTermCount { get; set; }

        public bool HasBeenSeen { get; set; }

        public string BroaderContext { get; set; }

        public List<string> Topics { get; set; }

        #endregion

        public static bool IncludeTopicNameInSummary { get; set; }

        #endregion

        public static MatchedTripReport Create(TripReport tripReport, List<MatchInfo> matches)
        {
            #region Sanity checks

            if (tripReport is null)
            {
                throw new ArgumentNullException(nameof(tripReport));
            }

            if (matches is null)
            {
                throw new ArgumentNullException(nameof(matches));
            }

            if (matches.Count == 0)
            {
                throw new ArgumentException($"{nameof(matches)} cannot be an empty list.");
            }

            #endregion

            MatchedTripReport matchInfo = new()
            {
                //Report = tripReport,
                SourceUri = tripReport.Url,
                Matches = MatchedTripReport.GetKeywords(matches),
                Title = tripReport.Title,
                Region = (tripReport as WtaTripReport)?.Region,
                TripDate = tripReport.TripDate,
                WordCount = matches[0].WordCount,
                HikeType = (tripReport as WtaTripReport)?.HikeType,
                BroaderContext = MatchedTripReport.GetExerpts(matches),
                Topics = matches.Where(m => !m.IsEmpty).Select(m => m.Topic.Name).ToList(),
            };

            return matchInfo;
        }

        private static string GetKeywords(List<MatchInfo> matchInfos)
        {
            StringBuilder keywords = new();

            Dictionary<string, int> consolidated = MatchedTripReport.ConsolidateMatches(matchInfos);

            foreach (KeyValuePair<string, int> word in consolidated)
            {
                keywords.Append(word.Key);

                if (word.Value > 1)
                {
                    keywords.Append($" {word.Value}x");
                }

                keywords.Append(",");
            }

            if (keywords.Length > 0)
            {
                keywords.Length -= 1;
            }

            return keywords.ToString();
        }

        private static string GetExerpts(List<MatchInfo> matchInfos)
        {
            StringBuilder exerpts = new();

            foreach (MatchInfo matchInfo in matchInfos)
            {
                if (!matchInfo.IsEmpty)
                {
                    if (MatchedTripReport.IncludeTopicNameInSummary)
                    {
                        exerpts.AppendLine(matchInfo.Topic.Name);
                    }

                    foreach (string quote in matchInfo.MatchQuotes)
                    {
                        exerpts.AppendLine(quote);
                    }

                    if (MatchedTripReport.IncludeTopicNameInSummary)
                    {
                        exerpts.AppendLine();
                    }
                }
            }

            return exerpts.ToString().Trim();
        }

        private static Dictionary<string, int> ConsolidateMatches(List<MatchInfo> matchInfos)
        {
            Dictionary<string, int> result = new();

            foreach (MatchInfo details in matchInfos)
            {
                foreach (var word in details.MatchCounts)
                {
                    if (!result.ContainsKey(word.Key))
                    {
                        result.Add(word.Key, word.Value);
                    }
                    else
                    {
                        result[word.Key] += 1;
                    }
                }
            }

            return result;
        }
    }
}
