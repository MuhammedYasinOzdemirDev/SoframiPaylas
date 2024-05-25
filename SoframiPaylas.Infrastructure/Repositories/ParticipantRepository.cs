using Google.Cloud.Firestore;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Infrastructure.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly FirestoreDb db;
        private readonly FirebaseService firebaseService;
        public ParticipantRepository(FirebaseService service)
        {
            db = service.GetDb();
            firebaseService = service;
        }
        public async Task<bool> AddParticipantAsync(Participant participant) //Katilma İstegi 
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                if (participant == null)
                    throw new ArgumentNullException(nameof(participant), "Participant object must not be null.");
                CollectionReference reference = db.Collection("Participants");
                await reference.AddAsync(participant);
                // DocumentReference'in null olup olmadığına bakarak işlem sonucunu kontrol ederiz
                return reference != null;
            }, TimeSpan.FromSeconds(20));
        }

        public async Task<bool> UpdateParticipantStatus(string postId, string userId, int status)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
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
            }, TimeSpan.FromSeconds(20));
        }
        public async Task<List<(Participant participant, string id, string userName)>> GetParticipantPostIdAsync(string postId)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
   {
       Query query = db.Collection("Participants").WhereEqualTo("postID", postId);
       QuerySnapshot snapshot = await query.GetSnapshotAsync();
       List<(Participant participant, string id, string userName)> participants = new List<(Participant participant, string id, string userName)>();

       foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
       {
           string id = documentSnapshot.Id;
           Participant participant = documentSnapshot.ConvertTo<Participant>();

           // `userID` ile `Users` tablosundan `Username` bilgisi alınır
           DocumentReference userDocRef = db.Collection("Users").Document(participant.UserID);
           DocumentSnapshot userSnapshot = await userDocRef.GetSnapshotAsync();
           string userName = userSnapshot.Exists ? userSnapshot.GetValue<string>("userName") : null;


           participants.Add((participant: participant, id: id, userName: userName));
       }

       return participants;
   }, TimeSpan.FromSeconds(20));
        }
        public async Task<bool> CheckIfRequestExistsAsync(string postId, string userId)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                Query query = db.Collection("Participants")
                                .WhereEqualTo("postID", postId)
                                .WhereEqualTo("userID", userId);
                QuerySnapshot snapshot = await query.GetSnapshotAsync();
                return snapshot.Documents.Count > 0;
            }, TimeSpan.FromSeconds(20));
        }
        public async Task<int> CheckRequestStatusAsync(string postId, string userId)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                Query query = db.Collection("Participants")
                                .WhereEqualTo("postID", postId)
                                .WhereEqualTo("userID", userId);
                QuerySnapshot snapshot = await query.GetSnapshotAsync();

                if (snapshot.Documents.Count == 0)
                {
                    return -1; // İstek yapılmamış
                }

                DocumentSnapshot documentSnapshot = snapshot.Documents.First();
                Participant participant = documentSnapshot.ConvertTo<Participant>();
                return participant.Status;
            }, TimeSpan.FromSeconds(20));
        }
        public async Task<bool> DeleteParticipantAsync(string id)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
                       {
                           if (string.IsNullOrEmpty(id))
                               throw new ArgumentException("Post ID cannot be null or empty.", nameof(id));

                           DocumentReference postRef = db.Collection("Participants").Document(id);

                           // Firestore'dan belirtilen kullanıcıyı silme
                           try
                           {
                               await postRef.DeleteAsync();
                               return true;  // Silme işlemi başarılı
                           }
                           catch (Exception ex)
                           {
                               // Loglama işlemi burada yapılabilir
                               return false;  // Silme işlemi başarısız
                           }
                       }, TimeSpan.FromSeconds(20));
        }

    }
}