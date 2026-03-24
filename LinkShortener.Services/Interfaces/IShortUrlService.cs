// LinkShortener.Services/Interfaces/IShortUrlService.cs
using LinkShortener.Common.DTOs;

namespace LinkShortener.Services.Interfaces
{
    public interface IShortUrlService
    {
        Task<CreateShortUrlResponseDto> CreateShortUrlAsync(string originalUrl, string baseUrl);
        Task<string?> GetOriginalUrlAsync(string shortCode);
        Task<List<ShortUrlDto>> GetAllAsync(string baseUrl);
        Task IncrementClickCountAsync(string shortCode);
        Task<bool> DeleteAsync(int id);
    }
}