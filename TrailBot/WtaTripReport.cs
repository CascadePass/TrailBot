using System.Collections.Generic;
using System.Text;

namespace CascadePass.TrailBot
{
    /// <summary>
    /// Represents a <see cref="TripReport"/> posted on Washington Trails Association.
    /// </summary>
    public class WtaTripReport : TripReport
    {
        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="WtaTripReport"/>.
        /// </summary>
        public WtaTripReport()
        {
            this.Source = SupportedTripReportSource.WashingtonTrailsAssociation;

            this.TrailConditions = new();
            this.Trails = new();
            this.Feature = new();
            this.ImageUrls = new();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the region of the trip report.
        /// </summary>
        /// <example>
        /// Regions include:  Central Cascades, Central Washington, Eastern Washington,
        /// Issaquah Alps, Mount Rainier Area, North Cascades, Olympic Peninsula,
        /// Puget Sound and Islands, Snoqualmie Region, South Cascades, Southwest Washington.
        /// </example>
        /// <remarks>
        /// This data comes from the web in all caps, and is converted locally to
        /// title case.
        /// </remarks>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the name of the account that wrote the trip report.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the type of activity being described in the trip report.
        /// </summary>
        /// <example>
        /// HikeTypes include: Day hike, Overnight, Multi-night backpack, Snowshoe/XC Ski
        /// </example>
        public string HikeType { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="WtaTrailCondition"/>s associated with the trip.
        /// </summary>
        public List<WtaTrailCondition> TrailConditions { get; set; }

        /// <summary>
        /// Gets the road conditions leading to the trail.
        /// </summary>
        /// <remarks>
        /// This information comes from the <see cref="TrailConditions"/> collection.
        /// </remarks>
        public string RoadConditions { get; set; }


        /// <summary>
        /// Gets the bug conditions during the activity.
        /// </summary>
        /// <remarks>
        /// This information comes from the <see cref="TrailConditions"/> collection.
        /// </remarks>
        public string Bugs { get; set; }


        /// <summary>
        /// Gets the snow conditions during the activity.
        /// </summary>
        /// <remarks>
        /// This information comes from the <see cref="TrailConditions"/> collection.
        /// </remarks>
        public string Snow { get; set; }

        /// <summary>
        /// Gets or sets a list of trail names involved in the trip report.
        /// </summary>
        public List<string> Trails { get; set; }

        /// <summary>
        /// Gets or sets a list of features associated with the trip.
        /// </summary>
        /// <remarks>
        /// Trail features are tags like "wildflowers blooming" that are applied to
        /// some trip reports.
        /// </remarks>
        public List<string> Feature { get; set; }

        /// <summary>
        /// Gets or sets a list of urls to images that appeared in the trip report.
        /// </summary>
        public List<string> ImageUrls { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the text of a trip report for search and matching purposes.
        /// </summary>
        /// <returns>A string containing the trip report.</returns>
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

        #endregion
    }
}
