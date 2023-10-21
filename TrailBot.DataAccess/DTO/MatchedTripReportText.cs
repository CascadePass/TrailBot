namespace CascadePass.TrailBot.DataAccess.DTO
{
    public class MatchedTripReportText : DataTransferObject
    {
        public long ID { get; set; }

        public long TripReportID { get; set; }

        public long TextID { get; set; }
    }
}
