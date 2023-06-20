using System;

namespace CascadePass.TrailBot
{
    public class SleepEventArgs : EventArgs
    {
        public TimeSpan Duration { get; set; }
    }
}
