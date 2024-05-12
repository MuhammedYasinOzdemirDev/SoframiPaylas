
namespace SoframiPaylas.Application.DTOs.Participant
{
    public class JoinParticipantDto
    {
        /// <summary>
        /// Katılımcının kullanıcı ID'si.
        /// </summary>
        /// <example>VNRPesXcUL01AOaqFouk</example>
        public string UserID { get; set; }
        /// <summary>
        /// Katılımcının durumu. Örneğin, 1 bekleyen, 2 onaylanan, 3 reddedilen anlamına gelebilir.
        /// </summary>
        /// <example>1</example>
        public int Status { get; set; }
    }
}