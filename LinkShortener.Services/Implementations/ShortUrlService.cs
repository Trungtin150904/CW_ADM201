// LinkShortener.Services/Implementations/ShortUrlService.cs
using LinkShortener.Common.DTOs;
using LinkShortener.Data.Entities;
using LinkShortener.Data.Interfaces;
using LinkShortener.Services.Interfaces;

namespace LinkShortener.Services.Implementations
{
    public class ShortUrlService : IShortUrlService
    {
        private readonly IShortUrlRepository _repository;

        public ShortUrlService(IShortUrlRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateShortUrlResponseDto> CreateShortUrlAsync(string originalUrl, string baseUrl)
        {
            if (!IsValidUrl(originalUrl))
                throw new ArgumentException("Invalid URL. Please enter the correct URL format (Ex: https://example.com)");

            string code;
            do
            {
                code = GenerateShortCode();
            } while (await _repository.CodeExistsAsync(code));

            var entity = new ShortUrl
            {
                OriginalUrl = originalUrl,
                ShortCode = code,
                CreatedAt = DateTime.Now,
                ClickCount = 0
            };

            await _repository.CreateAsync(entity);

            return new CreateShortUrlResponseDto
            {
                ShortCode = code,
                ShortLink = $"{baseUrl}/go/{code}",
                OriginalUrl = originalUrl
            };
        }

        public async Task<string?> GetOriginalUrlAsync(string shortCode)
        {
            var shortUrl = await _repository.GetByCodeAsync(shortCode);
            return shortUrl?.OriginalUrl;
        }

        public async Task<List<ShortUrlDto>> GetAllAsync(string baseUrl)
        {
            var list = await _repository.GetAllAsync();
            return list.Select(x => new ShortUrlDto
            {
                Id = x.Id,
                OriginalUrl = x.OriginalUrl,
                ShortCode = x.ShortCode,
                CreatedAt = x.CreatedAt,
                ClickCount = x.ClickCount,
                ShortLink = $"{baseUrl}/go/{x.ShortCode}"
            }).ToList();
        }

        public async Task IncrementClickCountAsync(string shortCode)
        {
            await _repository.IncrementClickCountAsync(shortCode);
        }

        private string GenerateShortCode()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var result)
                && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
        }
        // Thêm method này vào class ShortUrlService
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}