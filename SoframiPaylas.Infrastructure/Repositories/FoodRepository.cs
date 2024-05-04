using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Infrastructure.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly FirebaseService _service;
        public FoodRepository(FirebaseService service)
        {
            _service = service;
        }

        public async Task<string> CreateFoodAsync(Food food)
        {
            string foodId = Guid.NewGuid().ToString();

            await _service.GetDb().Collection("Foods").Document(foodId).SetAsync(food);

            return foodId;
        }

        public async Task<bool> DeleteFoodAsync(string foodId)
        {
            if (string.IsNullOrEmpty(foodId))
                throw new ArgumentException("Food ID cannot be null or empty.", nameof(foodId));

            DocumentReference foodRef = _service.GetDb().Collection("Foods").Document(foodId);
            try
            {
                // Firestore'dan belirtilen kullanıcıyı silme
                await foodRef.DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Loglama işlemi burada yapılabilir
                return false;  // Güncelleme işlemi başarısız
            }
        }

        public async Task<List<Food>> GetAllFoodsAsync()
        {
            if (_service == null || _service.GetDb() == null)
                throw new InvalidOperationException("Database service is not initialized properly.");
            CollectionReference foodRef = _service.GetDb().Collection("Foods");
            QuerySnapshot snapshots = await foodRef.GetSnapshotAsync();

            List<Food> foods = new List<Food>();
            foreach (DocumentSnapshot document in snapshots.Documents)
            {

                if (document.Exists)
                {
                    Dictionary<string, object> foodDict = document.ToDictionary();
                    var food = new Food
                    {
                        PostID = foodDict.ContainsKey("postID") ? foodDict["postID"].ToString() : null,
                        Title = foodDict.ContainsKey("title") ? foodDict["title"].ToString() : null,
                        Description = foodDict.ContainsKey("description") ? foodDict["description"].ToString() : null,
                        Images = foodDict.ContainsKey("images") ? foodDict["images"].ToString() : null,
                    };
                    foods.Add(food);
                }
            }
            return foods;
        }

        public async Task<Food> GetFoodByIdAsync(string id)
        {
            if (_service == null || _service.GetDb() == null)
            {
                throw new InvalidOperationException("Database service is not initialized.");
            }

            DocumentReference eventRef = _service.GetDb().Collection("Foods").Document(id);
            DocumentSnapshot snapshot = await eventRef.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return null;  // Eğer belge yoksa null döner
            }

            Dictionary<string, object> foodDict = snapshot.ToDictionary();
            return new Food
            {
                PostID = foodDict.ContainsKey("postID") ? foodDict["postID"].ToString() : null,
                Title = foodDict.ContainsKey("title") ? foodDict["title"].ToString() : null,
                Description = foodDict.ContainsKey("description") ? foodDict["description"].ToString() : null,
                Images = foodDict.ContainsKey("images") ? foodDict["images"].ToString() : null,
            };
        }

        public async Task<bool> UpdateFoodAsync(Food food, string foodId)
        {
            if (food == null)
                throw new ArgumentNullException(nameof(food), "Food object must not be null.");
            if (string.IsNullOrEmpty(foodId))
                throw new ArgumentException("Food ID must not be null or empty.", nameof(foodId));

            DocumentReference foodReference = _service.GetDb().Collection("Foods").Document(foodId);
            try
            {
                await foodReference.SetAsync(food, SetOptions.MergeAll);
                return true;
            }
            catch (Exception ex)
            {
                // Loglama işlemi burada yapılabilir
                return false;  // Güncelleme işlemi başarısız
            }
        }
    }
}