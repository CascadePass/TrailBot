using System.Collections.Generic;
using System.Text;

namespace CascadePass.TrailBot
{
    public class WtaTripReport : TripReport
    {
        public WtaTripReport()
        {
            this.Source = SupportedTripReportSource.WashingtonTrailsAssociation;

            this.TrailConditions = new();
            this.Trails = new();
            this.Feature = new();
            this.ImageUrls = new();
        }

        public string Region { get; set; }
        public string Author { get; set; }
        public string HikeType { get; set; }
        public List<WtaTrailCondition> TrailConditions { get; set; }
        public string RoadConditions { get; set; }
        public string Bugs { get; set; }
        public string Snow { get; set; }
        public List<string> Trails { get; set; }
        public List<string> Feature { get; set; }
        public List<string> ImageUrls { get; set; }

        public override string GetSearchableReportText()
        {
            StringBuilder stringBuilder = new();

            foreach (string feature in this.Feature)
            {
                if (!string.IsNullOrEmpty(feature))
                {
                    stringBuilder.AppendLine(feature);
                }
            }

            foreach (var condition in this.TrailConditions)
            {
                if (!string.IsNullOrEmpty(condition.Description))
                {
                    stringBuilder.AppendLine(condition.Description);
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
