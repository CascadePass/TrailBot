namespace CascadePass.TrailBot.DataAccess.DTO
{
    public class ImageUrl
    {
        public long ID { get; set; }

        public string Address { get; set; }

        public long? ImageWidth { get; set; }

        public long? ImageHeight { get; set; }

        public long? FileSize { get; set; }

        public string Comments { get; set; }
    }
}
