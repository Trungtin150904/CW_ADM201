using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LinkShortener.Common.DTOs;
using LinkShortener.Services.Implementations;
using System.Security.Claims;

namespace StudentManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenService _tokenService;

        public AuthController(
            UserManager<IdentityUser> userManager,
            TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        /// <summary>POST api/auth/register — Đăng ký tài khoản</summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { error = "Email và Password không được để trống." });

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return BadRequest(new { error = "Email has already been used." });

            var user = new IdentityUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

            // Gán role User mặc định
            await _userManager.AddToRoleAsync(user, "User");

            return Ok(new { message = "Registration successful!" });
        }

        /// <summary>POST api/auth/login — Đăng nhập, nhận Access + Refresh Token</summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized(new { error = "Incorrect email or password." });

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordValid)
                return Unauthorized(new { error = "Incorrect email or password." });

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);

            return Ok(new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(15),
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? "User"
            });
        }

        /// <summary>POST api/auth/refresh — Lấy Access Token mới bằng Refresh Token</summary>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
        {
            var (accessToken, newRefreshToken, error) =
                await _tokenService.RefreshAccessTokenAsync(dto.RefreshToken);

            if (error != null)
                return Unauthorized(new { error });

            return Ok(new
            {
                accessToken,
                refreshToken = newRefreshToken,
                accessTokenExpiry = DateTime.UtcNow.AddMinutes(15)
            });
        }

        /// <summary>POST api/auth/logout — Thu hồi tất cả Refresh Token</summary>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
                await _tokenService.RevokeAllRefreshTokensAsync(userId);

            return Ok(new { message = "Logout successful." });
        }

        /// <summary>GET api/auth/me — Xem thông tin user hiện tại</summary>
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            return Ok(new
            {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier),
                email = User.FindFirstValue(ClaimTypes.Email),
                role = User.FindFirstValue(ClaimTypes.Role)
            });
        }
    }
}