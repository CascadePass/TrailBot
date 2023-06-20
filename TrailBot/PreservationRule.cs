namespace CascadePass.TrailBot
{
    /// <summary>
    /// Specifies which content should be saved to disc.
    /// </summary>
    public enum PreservationRule
    {
        /// <summary>
        /// Value has not been set, default value of <see cref="Matching"/> will be used.
        /// </summary>
        ValueNotSet,

        /// <summary>
        /// Save every <see cref="TripReport"/> processed, whether it matches any
        /// <see cref="Topic"/> or not.
        /// </summary>
        All,

        /// <summary>
        /// Save <see cref="TripReport"/>s to disc only if they match a <see cref="Topic"/>.
        /// </summary>
        Matching,

        /// <summary>
        /// Do not save any <see cref="TripReport"/>s to disc.
        /// </summary>
        None
    }
}
