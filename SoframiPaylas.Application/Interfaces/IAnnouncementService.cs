using SoframiPaylas.Application.DTOs.Announcement;

namespace SoframiPaylas.Application.Interfaces;
public interface IAnnouncementService
{
    Task<AnnouncementDto> AddAnnouncementAsync(CreateAnnouncementDto announcementDto);
    Task<IEnumerable<AnnouncementDto>> GetAnnouncementsAsync();
}