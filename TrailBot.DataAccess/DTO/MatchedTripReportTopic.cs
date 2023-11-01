namespace CascadePass.TrailBot.DataAccess.DTO
{
    public class MatchedTripReportTopic : DataTransferObject
    {
        public long ID { get; set; }

        public long TripReportID { get; set; }

        public long TopicID { get; set; }

        public string Exerpt { get; set; }
    }
}
