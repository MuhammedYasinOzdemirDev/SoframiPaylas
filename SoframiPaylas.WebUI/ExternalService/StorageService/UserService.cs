namespace SoframiPaylas.WebUI.ExternalService.StorageService;
using SoframiPaylas.WebUI.Models;
using SoframiPaylas.WebUI.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

public interface IUserService
{
    UserProfileViewModel GetUser();
    String GetUserId();
    Task<HttpResponseMessage> UpdateUser(UserProfileViewModel user);
    Task SetUser(string userId);
}

public class UserService : IUserService
{
    private UserProfileViewModel _currentUser;
    private string _currentUserId;
    private readonly IUserApiService _userApiService;
    private readonly IMemoryCache _cache;
    private const string UserCacheKeyPrefix = "User_";

    public UserService(IUserApiService userApiService, IMemoryCache cache)
    {
        _userApiService = userApiService;
        _cache = cache;
    }

    public UserProfileViewModel GetUser()
    {
        // Kullanıcı bilgisini döndürür
        return _currentUser;
    }
    public String GetUserId()
    {
        return _currentUserId;
    }

    public async Task<HttpResponseMessage> UpdateUser(UserProfileViewModel user)
    {
        // Kullanıcı bilgisini günceller ve cache'i de günceller
        _currentUser = user;
        if (!string.IsNullOrEmpty(_currentUserId))
        {
            _cache.Set(UserCacheKeyPrefix + _currentUserId, _currentUser, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) // Cache süresi 1 saat
            });
        }
        return await _userApiService.UpdateUserProfileAsync(_currentUser, _currentUserId);
    }

    public async Task SetUser(string userId)
    {
        // Eğer kullanıcı ID'si değişmediyse cache'deki kullanıcı bilgisini döndür
        if (_currentUserId == userId && _cache.TryGetValue(UserCacheKeyPrefix + userId, out UserProfileViewModel cachedUser))
        {
            _currentUser = cachedUser;
        }
        else
        {
            // Kullanıcı bilgisi cache'de yoksa veritabanından getir ve cache'e ekle
            _currentUser = await _userApiService.GetUserProfileAsync(userId);
            _currentUserId = userId;
            _cache.Set(UserCacheKeyPrefix + userId, _currentUser, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) // Cache süresi 1 saat
            });
        }
    }
}
