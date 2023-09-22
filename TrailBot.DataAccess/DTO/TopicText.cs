namespace CascadePass.TrailBot.DataAccess.DTO
{
    public class TopicText
    {
        public long ID { get; set; }

        public long TopicID { get; set; }

        public long TextID { get; set; }
    }

    public class MatchedTripReportTopic
    {
        public long ID { get; set; }

        public long TripReportID { get; set; }

        public long TopicID { get; set; }

        public string Exerpt { get; set; }
    }

    public class MatchedTripReportText
    {
        public long ID { get; set; }

        public long TripReportID { get; set; }

        public long TextID { get; set; }
    }
}
