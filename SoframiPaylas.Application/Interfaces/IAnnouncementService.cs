using SoframiPaylas.Application.DTOs.Announcement;

namespace SoframiPaylas.Application.Interfaces;
public interface IAnnouncementService
{
    Task<AnnouncementDto> AddAnnouncementAsync(string postId, CreateAnnouncementDto announcementDto);
    Task<IEnumerable<AnnouncementDto>> GetAnnouncementsAsync();
    Task<IEnumerable<AnnouncementDto>> GetPostIdAnnouncementsAsync(string postId);
    Task<IEnumerable<AnnouncementDto>> GetPostIds(List<string> postIds);
}