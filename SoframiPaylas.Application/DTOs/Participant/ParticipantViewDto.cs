
namespace SoframiPaylas.Application.DTOs.Participant
{
    public class ParticipantViewDto
    {
        public string ParticipantId { get; set; }
        public int Status { get; set; }
        public string UserID { get; set; }

        public string PostID { get; set; }
        public string? UserName { get; set; }
    }
}