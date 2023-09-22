using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.TrailBot.DataAccess.DTO
{
    public class WtaTripReport
    {
        public long ID { get; set; }

        public Url Url { get; set; }

        public string Title { get; set; }

        public string Region { get; set; }

        public string HikeType { get; set; }

        public string Author { get; set; }

        public DateTime TripDate { get; set; }

        public DateTime PublishedDate { get; set; }

        public DateTime ProcessedDate { get; set; }

        public string ReportText { get; set; }
    }
}
