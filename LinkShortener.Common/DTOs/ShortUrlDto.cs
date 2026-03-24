using System;

namespace LinkShortener.Common.DTOs
{
    public class ShortUrlDto
    {
        public int Id { get; set; }
        public string? OriginalUrl { get; set; }
        public string? ShortCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ClickCount { get; set; }
        public string? ShortLink { get; set; } 
    }

    public class CreateShortUrlDto
    {
        public string OriginalUrl { get; set; } = string.Empty;
    }

    public class CreateShortUrlResponseDto
    {
        public string ShortCode { get; set; } = string.Empty;
        public string ShortLink { get; set; } = string.Empty;
        public string OriginalUrl { get; set; } = string.Empty;
    }
}