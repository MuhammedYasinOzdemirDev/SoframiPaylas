namespace SoframiPaylas.Application.Services;
using AutoMapper;
using Google.Cloud.Firestore;
using SoframiPaylas.Application.DTOs.Announcement;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AnnouncementService : IAnnouncementService
{
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly IMapper _mapper;

    public AnnouncementService(IAnnouncementRepository announcementRepository, IMapper mapper)
    {
        _announcementRepository = announcementRepository;
        _mapper = mapper;
    }

    public async Task<AnnouncementDto> AddAnnouncementAsync(CreateAnnouncementDto announcementDto)
    {
        var announcement = _mapper.Map<Announcement>(announcementDto);
        announcement.Timestamp = Timestamp.FromDateTime(DateTime.UtcNow);
        var result = await _announcementRepository.AddAnnouncementAsync(announcement);
        var dto = _mapper.Map<AnnouncementDto>(result.announcement);
        dto.Id = result.announcementId;
        return dto;
    }

    public async Task<IEnumerable<AnnouncementDto>> GetAnnouncementsAsync()
    {
        var announcements = await _announcementRepository.GetAnnouncementsAsync();
        return announcements.Select(a =>
        {
            var dto = _mapper.Map<AnnouncementDto>(a.announcement);
            dto.Id = a.announcementId;
            return dto;
        });
    }
}
