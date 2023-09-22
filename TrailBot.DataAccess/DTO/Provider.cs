using System;

namespace CascadePass.TrailBot.DataAccess.DTO
{
    public class Provider
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string TypeName { get; set; }

        public int PreservationRule { get; set; }

        public int State { get; set; }

        public int Browser { get; set; }

        public DateTime? LastTripReportRequest { get; set; }

        public DateTime? LastGetRecentRequest { get; set; }

        public string ProviderXml { get; set; }
    }
}
