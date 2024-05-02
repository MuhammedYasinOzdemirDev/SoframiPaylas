using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace SoframiPaylas.Application.DTOs.Post
{
    public class UpdatePostDto
    {
        public string PostId { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Timestamp Date { get; set; }
        public int Participants { get; set; }
        public string Images { get; set; }
        public string Status { get; set; }
    }
}