using System;
using System.Xml.Serialization;

namespace CascadePass.TrailBot
{
    /// <summary>
    /// Represents a detail about the current state of the trail that
    /// readers should be aware of.
    /// </summary>
    [Serializable]
    public class WtaTrailCondition
    {
        /// <summary>
        /// Gets or sets the Title of the <see cref="WtaTrailCondition"/>.
        /// </summary>
        /// <remarks>
        /// Examples:  Snow, Bugs, etc
        /// </remarks>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a text description of the <see cref="WtaTrailCondition"/>.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the raw text of the <see cref="WtaTrailCondition"/>.  This
        /// gets parsed into <see cref="Title"/> and <see cref="Description"/>
        /// which have cleaner, more useful data.
        /// </summary>
        [XmlIgnore]
        public string Text { get; set; }

        /// <summary>
        /// Gets a text representation of the <see cref="WtaTrailCondition"/> data.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(this.Text))
            {
                return this.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.Title) && !string.IsNullOrWhiteSpace(this.Description))
            {
                return $"{this.Title}: {this.Description}";
            }

            if (!string.IsNullOrWhiteSpace(this.Title))
            {
                return this.Title;
            }

            if (!string.IsNullOrWhiteSpace(this.Description))
            {
                return this.Description;
            }

            return base.ToString();
        }
    }
}
