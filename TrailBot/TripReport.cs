using System;
using System.Text;

namespace CascadePass.TrailBot
{
    public class TripReport
    {
        public SupportedTripReportSource Source { get; set; }

        public string Url { get; set; }
        public DateTime TripDate { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime FoundDate { get; set; }
        public DateTime ProcessedDate { get; set; }
        public string Title { get; set; }
        public string ReportText { get; set; }

        /// <summary>
        /// Gets the text of a trip report for search and matching purposes.
        /// </summary>
        /// <returns>A string containing the trip report.</returns>
        public virtual string GetSearchableReportText()
        {
            StringBuilder stringBuilder = new();

            if (!string.IsNullOrEmpty(this.Title))
            {
                stringBuilder.AppendLine(this.Title);
            }

            if (!string.IsNullOrEmpty(this.ReportText))
            {
                stringBuilder.AppendLine(this.ReportText);
            }

            return stringBuilder.ToString();
        }

    }
}
