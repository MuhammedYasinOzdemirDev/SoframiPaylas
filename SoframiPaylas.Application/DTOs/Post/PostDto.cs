using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using SoframiPaylas.Application.DTOs.Food;
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Application.DTOs.Post
{
    public class PostDto
    {

        public string HostID { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public GeoPoint Location { get; set; }

        public string FormattedDate { get; set; }

        public string Time { get; set; }

        public int MaxParticipants { get; set; }
        public List<string> Images { get; set; }

        public bool PostStatus { get; set; }

        public List<string> RelatedFoods { get; set; }


        public List<string> Participants { get; set; }
    }
}