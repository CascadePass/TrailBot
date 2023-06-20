using System;
using System.Text;

namespace CascadePass.TrailBot
{
    public class TripReport
    {
        public SupportedTripReportSource Source { get; set; }

        public string Url { get; set; }
        public DateTime TripDate { get; set; }
        public string Title { get; set; }
        public string ReportText { get; set; }

        public virtual string GetSearchableReportText()
        {
            StringBuilder stringBuilder = new();

            if (true)
            {
                if (!string.IsNullOrEmpty(this.Title))
                {
                    stringBuilder.AppendLine(this.Title);
                }
            }

            if (!string.IsNullOrEmpty(this.ReportText))
            {
                stringBuilder.AppendLine(this.ReportText);
            }

            return stringBuilder.ToString();
        }

    }
}
