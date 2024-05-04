using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace SoframiPaylas.Application.DTOs.Food
{
    public class UpdateFoodDto
    {
        public string PostID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Images { get; set; }
    }
}