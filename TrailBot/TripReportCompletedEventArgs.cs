using System;

namespace CascadePass.TrailBot
{
    public class TripReportCompletedEventArgs : EventArgs
    {
        public TripReport TripReport { get; set; }
    }
}
