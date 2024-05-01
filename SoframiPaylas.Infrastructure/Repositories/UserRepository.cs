using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Google.Cloud.Firestore;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly FirebaseService _service;
        public UserRepository(FirebaseService service)
        {
            _service = service;
        }
        public async Task<User> GetUserAsync()
        {


            try
            {
                string documentId = "tJoP8UsKxVpRAnCuORqY"; // Belge ID'si
                if (string.IsNullOrEmpty(documentId))
                    throw new ArgumentException("Document ID cannot be null or empty.");

                DocumentReference docRef = _service.GetDb().Collection("Users").Document(documentId);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    User user = new User { FullName = snapshot.GetValue<string>("fullname") };
                    return user;
                }
                else
                {
                    throw new InvalidOperationException("No user found with the specified document ID.");
                }
            }

            catch (Exception ex)
            {
                // Diğer genel hatalar için loglama
                throw new Exception("An error occurred.", ex);
            }
        }
    }
}