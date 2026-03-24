using LinkShortener.Data.Entities;

namespace LinkShortener.Data.Interfaces
{
    public interface IShortUrlRepository
    {
        Task<ShortUrl?> GetByCodeAsync(string shortCode);
        Task<List<ShortUrl>> GetAllAsync();
        Task<ShortUrl> CreateAsync(ShortUrl shortUrl);
        Task IncrementClickCountAsync(string shortCode);
        Task<bool> CodeExistsAsync(string shortCode);
        Task<bool> DeleteAsync(int id);
    }
}