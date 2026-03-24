using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using LinkShortener.Data;
using LinkShortener.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LinkShortener.Services.Implementations
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly LinkShortenerDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TokenService(
            IConfiguration config,
            LinkShortenerDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _config = config;
            _context = context;
            _userManager = userManager;
        }

        // ===== Tạo Access Token (JWT) =====
        public async Task<string> GenerateAccessTokenAsync(IdentityUser user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"]!;
            var issuer = jwtSettings["Issuer"]!;
            var audience = jwtSettings["Audience"]!;
            var expMinutes = int.Parse(jwtSettings["AccessTokenExpirationMinutes"]!);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Thêm roles vào claims
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ===== Tạo Refresh Token =====
        public async Task<string> GenerateRefreshTokenAsync(string userId)
        {
            var expDays = int.Parse(_config["JwtSettings:RefreshTokenExpirationDays"]!);

            // Tạo random token
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var token = Convert.ToBase64String(randomBytes);

            // Lưu vào DB
            var refreshToken = new RefreshToken
            {
                Token = token,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(expDays),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false,
                IsUsed = false
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return token;
        }

        // ===== Refresh Access Token =====
        public async Task<(string? AccessToken, string? NewRefreshToken, string? Error)>
            RefreshAccessTokenAsync(string refreshToken)
        {
            // Tìm refresh token trong DB
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (storedToken == null)
                return (null, null, "Refresh token không tồn tại.");

            if (storedToken.IsRevoked)
                return (null, null, "Refresh token đã bị thu hồi.");

            if (storedToken.IsUsed)
                return (null, null, "Refresh token đã được sử dụng.");

            if (storedToken.ExpiresAt < DateTime.UtcNow)
                return (null, null, "Refresh token đã hết hạn. Vui lòng đăng nhập lại.");

            // Đánh dấu token cũ đã dùng
            storedToken.IsUsed = true;
            await _context.SaveChangesAsync();

            // Tìm user
            var user = await _userManager.FindByIdAsync(storedToken.UserId);
            if (user == null)
                return (null, null, "User không tồn tại.");

            // Tạo token mới
            var newAccessToken = await GenerateAccessTokenAsync(user);
            var newRefreshToken = await GenerateRefreshTokenAsync(user.Id);

            return (newAccessToken, newRefreshToken, null);
        }

        // ===== Revoke tất cả Refresh Token của user (khi logout) =====
        public async Task RevokeAllRefreshTokensAsync(string userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ToListAsync();

            foreach (var token in tokens)
                token.IsRevoked = true;

            await _context.SaveChangesAsync();
        }
    }
}