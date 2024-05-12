using Google.Cloud.Firestore;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Infrastructure.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly FirestoreDb db;
        public ParticipantRepository(FirebaseService service)
        {
            db = service.GetDb();
        }
        public async Task<bool> AddParticipantAsync(Participant participant) //Katilma İstegi 
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant), "Participant object must not be null.");
            CollectionReference reference = db.Collection("Participants");
            await reference.AddAsync(participant);
            // DocumentReference'in null olup olmadığına bakarak işlem sonucunu kontrol ederiz
            return reference != null;
        }

        public async Task<bool> UpdateParticipantStatus(string postId, string userId, int status)
        {
            Query query = db.Collection("Participants").
            WhereEqualTo("postID", postId).
            WhereEqualTo("userID", userId);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            if (snapshot.Count == 0)
                return false;

            DocumentSnapshot documentSnapshot = snapshot.Documents[0];
            Dictionary<string, object> updates = new Dictionary<string, object>{
            {"status",status},};
            await documentSnapshot.Reference.UpdateAsync(updates);
            return true;
        }
    }
}