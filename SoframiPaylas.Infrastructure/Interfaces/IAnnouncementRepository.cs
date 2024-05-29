using SoframiPaylas.Domain.Entities;
namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface IAnnouncementRepository
    {
        Task<(Announcement announcement, string announcementId)> AddAnnouncementAsync(Announcement announcement);
        Task<List<(Announcement announcement, string announcementId)>> GetAnnouncementsAsync();
        Task<List<(Announcement announcement, string announcementId)>> GetPostIdAnnouncementsAsync(string postId);
        Task<List<(Announcement announcement, string id)>> GetPostIds(List<string> postIds);
    }

}