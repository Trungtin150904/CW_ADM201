// LinkShortener.Data/Implementations/ShortUrlRepository.cs
using Microsoft.EntityFrameworkCore;
using LinkShortener.Data.Entities;
using LinkShortener.Data.Interfaces;

namespace LinkShortener.Data.Implementations
{
    public class ShortUrlRepository : IShortUrlRepository
    {
        private readonly LinkShortenerDbContext _context;

        public ShortUrlRepository(LinkShortenerDbContext context)
        {
            _context = context;
        }

        public async Task<ShortUrl?> GetByCodeAsync(string shortCode)
        {
            return await _context.ShortUrls
                .FirstOrDefaultAsync(x => x.ShortCode == shortCode);
        }

        public async Task<List<ShortUrl>> GetAllAsync()
        {
            return await _context.ShortUrls
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<ShortUrl> CreateAsync(ShortUrl shortUrl)
        {
            _context.ShortUrls.Add(shortUrl);
            await _context.SaveChangesAsync();
            return shortUrl;
        }

        public async Task IncrementClickCountAsync(string shortCode)
        {
            var shortUrl = await _context.ShortUrls
                .FirstOrDefaultAsync(x => x.ShortCode == shortCode);
            if (shortUrl != null)
            {
                shortUrl.ClickCount++;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CodeExistsAsync(string shortCode)
        {
            return await _context.ShortUrls
                .AnyAsync(x => x.ShortCode == shortCode);
        }
        // Thêm vào class ShortUrlRepository
        public async Task<bool> DeleteAsync(int id)
        {
            var shortUrl = await _context.ShortUrls.FindAsync(id);
            if (shortUrl == null) return false;

            _context.ShortUrls.Remove(shortUrl);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}