
using SoframiPaylas.Application.DTOs;


namespace SoframiPaylas.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUserAsync();
        Task<UserDto> GetUserByIdAsync(string userId);

        Task UpdateUserAsync(UpdateUserDto userDto, string userId);
        Task DeleteUserAsync(string userId);
    }
}