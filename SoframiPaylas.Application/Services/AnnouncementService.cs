namespace SoframiPaylas.Application.Services;
using AutoMapper;
using Google.Cloud.Firestore;
using SoframiPaylas.Application.DTOs.Announcement;
using SoframiPaylas.Application.ExternalServices.Interfaces;
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
    private readonly IEmailSender _emailSender;
    private readonly IParticipantRepository _participantRepository;

    public AnnouncementService(IAnnouncementRepository announcementRepository, IMapper mapper, IEmailSender emailSender, IParticipantRepository participantRepository)
    {
        _announcementRepository = announcementRepository;
        _mapper = mapper;
        _emailSender = emailSender;
        _participantRepository = participantRepository;
    }

    public async Task<AnnouncementDto> AddAnnouncementAsync(string postId, CreateAnnouncementDto announcementDto)
    {
        var announcement = _mapper.Map<Announcement>(announcementDto);
        announcement.Timestamp = Timestamp.FromDateTime(DateTime.UtcNow);
        announcement.PostId = postId;
        var result = await _announcementRepository.AddAnnouncementAsync(announcement);
        var dto = _mapper.Map<AnnouncementDto>(result.announcement);
        dto.Id = result.announcementId;

        var participantEmails = await _participantRepository.GetParticipantEmailsByPostIdAsync(postId);
        string subject = "Yeni Duyuru: ";
        string message = announcementDto.Content;

        if (participantEmails.Count > 0)
            await _emailSender.SendEmailsAsync(participantEmails, subject, message);
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

    public async Task<IEnumerable<AnnouncementDto>> GetPostIdAnnouncementsAsync(string postId)
    {
        var announcements = await _announcementRepository.GetPostIdAnnouncementsAsync(postId);
        return announcements.Select(a =>
        {
            var dto = _mapper.Map<AnnouncementDto>(a.announcement);
            dto.Id = a.announcementId;
            return dto;
        });
    }

    public async Task<IEnumerable<AnnouncementDto>> GetPostIds(List<string> postIds)
    {
        var announcements = await _announcementRepository.GetPostIds(postIds);
        return announcements.Select(a =>
        {
            var dto = _mapper.Map<AnnouncementDto>(a.announcement);
            dto.Id = a.id;
            return dto;
        });
    }
}
