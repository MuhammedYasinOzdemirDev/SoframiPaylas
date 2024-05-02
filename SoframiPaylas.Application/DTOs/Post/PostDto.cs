using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoframiPaylas.Application.DTOs.Post
{
    public class PostDto
    {
        public string UserID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Participants { get; set; }
        public string Images { get; set; }
        public string Status { get; set; }
    }
}