namespace SoframiPaylas.Application.Services;
using AutoMapper;
using FirebaseAdmin.Messaging;
using Google.Cloud.Firestore;
using SoframiPaylas.Application.DTOs.Message;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;
    private readonly FirebaseMessagingService _firebaseMessagingService;

    public MessageService(IMessageRepository messageRepository, IMapper mapper, FirebaseMessagingService firebaseMessagingService)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
        _firebaseMessagingService = firebaseMessagingService;
    }

    public async Task<MessageDto> AddMessageAsync(CreateMessageDto messageDto)
    {
        var message = _mapper.Map<MessageStore>(messageDto);
        message.Timestamp = Timestamp.FromDateTime(DateTime.UtcNow);

        var result = await _messageRepository.AddMessageAsync(message);
        var dto = _mapper.Map<MessageDto>(result.message);
        dto.Id = result.messageId;

        return dto;
    }

    public async Task<IEnumerable<MessageDto>> GetMessagesByReceiverIdAsync(string receiverId)
    {
        var messages = await _messageRepository.GetMessagesByReceiverIdAsync(receiverId);
        return messages.Select(m =>
        {
            var dto = _mapper.Map<MessageDto>(m.message);
            dto.Id = m.messageId;
            return dto;
        });
    }

    public async Task<int> MessageCount(string userId)
    {
        return await _messageRepository.MessageCount(userId);
    }
}
