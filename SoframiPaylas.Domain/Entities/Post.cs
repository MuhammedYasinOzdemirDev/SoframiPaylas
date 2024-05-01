using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoframiPaylas.Domain.Entities
{
    public class Post
    {
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public int Participants { get; set; }
        public string Images { get; set; }
        public string Status { get; set; }

        // Navigation properties
        public User User { get; set; }
    }
}