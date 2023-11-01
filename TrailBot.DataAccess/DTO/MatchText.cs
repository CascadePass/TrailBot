namespace CascadePass.TrailBot.DataAccess.DTO
{
    public class MatchText : DataTransferObject
    {
        public long ID { get; set; }

        public long? ParentID { get; set; }

        public string Text { get; set; }
    }
}
