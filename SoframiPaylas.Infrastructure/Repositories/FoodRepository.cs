using Google.Cloud.Firestore;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Infrastructure.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly FirestoreDb db;
        private readonly FirebaseService firebaseService;
        public FoodRepository(FirebaseService service)
        {
            db = service.GetDb();
            firebaseService = service;
        }

        public async Task<string> CreateFoodAsync(Food food)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
                {
                    string foodId = Guid.NewGuid().ToString();

                    await db.Collection("Foods").Document(foodId).SetAsync(food);

                    return foodId;
                }, TimeSpan.FromSeconds(20));
        }

        public async Task<bool> DeleteFoodAsync(string foodId)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
                {
                    if (string.IsNullOrEmpty(foodId))
                        throw new ArgumentException("Food ID cannot be null or empty.", nameof(foodId));

                    DocumentReference foodRef = db.Collection("Foods").Document(foodId);
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
                }, TimeSpan.FromSeconds(20));
        }

        public async Task<List<Food>> GetAllFoodsAsync()
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
                {
                    if (db == null)
                        throw new InvalidOperationException("Database service is not initialized properly.");
                    CollectionReference foodRef = db.Collection("Foods");
                    QuerySnapshot snapshots = await foodRef.GetSnapshotAsync();

                    List<Food> foods = new List<Food>();
                    foreach (DocumentSnapshot document in snapshots.Documents)
                    {

                        if (document.Exists)
                        {
                            Dictionary<string, object> foodDict = document.ToDictionary();
                            var food = new Food
                            {

                                Title = foodDict.ContainsKey("title") ? foodDict["title"].ToString() : null,
                                Description = foodDict.ContainsKey("description") ? foodDict["description"].ToString() : null,

                            };
                            foods.Add(food);
                        }
                    }
                    return foods;
                }, TimeSpan.FromSeconds(20));
        }

        public async Task<Food> GetFoodByIdAsync(string id)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
                {
                    if (db == null)
                    {
                        throw new InvalidOperationException("Database service is not initialized.");
                    }

                    DocumentReference eventRef = db.Collection("Foods").Document(id);
                    DocumentSnapshot snapshot = await eventRef.GetSnapshotAsync();

                    if (!snapshot.Exists)
                    {
                        return null;  // Eğer belge yoksa null döner
                    }

                    Dictionary<string, object> foodDict = snapshot.ToDictionary();
                    return new Food
                    {

                        Title = foodDict.ContainsKey("title") ? foodDict["title"].ToString() : null,
                        Description = foodDict.ContainsKey("description") ? foodDict["description"].ToString() : null,

                    };
                }, TimeSpan.FromSeconds(20));
        }

        public async Task<bool> UpdateFoodAsync(Food food, string foodId)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
                {
                    if (food == null)
                        throw new ArgumentNullException(nameof(food), "Food object must not be null.");
                    if (string.IsNullOrEmpty(foodId))
                        throw new ArgumentException("Food ID must not be null or empty.", nameof(foodId));

                    DocumentReference foodReference = db.Collection("Foods").Document(foodId);
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
                }, TimeSpan.FromSeconds(20));
        }
    }
}