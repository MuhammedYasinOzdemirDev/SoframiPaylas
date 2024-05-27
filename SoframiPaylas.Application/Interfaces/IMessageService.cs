using SoframiPaylas.Application.DTOs.Message;

namespace SoframiPaylas.Application.Interfaces;
public interface IMessageService
{
    Task<MessageDto> AddMessageAsync(CreateMessageDto messageDto);
    Task<IEnumerable<MessageDto>> GetMessagesByReceiverIdAsync(string receiverId);
}