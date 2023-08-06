namespace CascadePass.TrailBot
{
    /// <summary>
    /// Represents part of a piece of text, which matches a topic.
    /// </summary>
    public class Exerpt
    {
        /// <summary>
        /// Gets or sets the <see cref="Topic"/>.
        /// </summary>
        public Topic Topic { get; set; }

        /// <summary>
        /// Gets or sets the text surrounding a keyword or -phrase.
        /// </summary>
        public string Quote { get; set; }
    }
}
