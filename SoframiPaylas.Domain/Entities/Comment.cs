using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoframiPaylas.Domain.Entities
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int UserID { get; set; }
        public int PostID { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Post Post { get; set; }
    }
}