using System.Collections.Generic;

namespace CascadePass.TrailBot.DataAccess.DTO
{
    public class Topic : DataTransferObject
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public List<TopicText> MatchText { get; set; }
    }
}
