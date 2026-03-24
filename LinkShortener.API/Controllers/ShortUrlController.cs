// StudentManager.API/Controllers/ShortUrlController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LinkShortener.Common.DTOs;
using LinkShortener.Services.Interfaces;

namespace StudentManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShortUrlController : ControllerBase
    {
        private readonly IShortUrlService _service;

        public ShortUrlController(IShortUrlService service)
        {
            _service = service;
        }

        /// <summary>GET api/shorturl — Lấy toàn bộ danh sách</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var list = await _service.GetAllAsync(baseUrl);
            return Ok(list);
        }

        /// <summary>GET api/shorturl/{code} — Lấy theo code</summary>
        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var originalUrl = await _service.GetOriginalUrlAsync(code);
            if (originalUrl == null)
                return NotFound(new { error = "Short URL không tồn tại." });

            return Ok(new { shortCode = code, originalUrl });
        }

        /// <summary>POST api/shorturl — Tạo short URL mới</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShortUrlDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.OriginalUrl))
                return BadRequest(new { error = "URL không được để trống." });

            try
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var result = await _service.CreateShortUrlAsync(dto.OriginalUrl, baseUrl);
                return CreatedAtAction(nameof(GetByCode), new { code = result.ShortCode }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>DELETE api/shorturl/{id} — Xóa short URL</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(new { error = "Short URL không tồn tại." });

            return Ok(new { message = "Deletion successful." });
        }

        /// <summary>GET /go/{code} — Redirect đến URL gốc</summary>
        [AllowAnonymous]
        [HttpGet("/go/{code}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> RedirectToUrl(string code)
        {
            await _service.IncrementClickCountAsync(code);
            var originalUrl = await _service.GetOriginalUrlAsync(code);

            if (originalUrl == null)
                return NotFound(new { error = "Short URL không tồn tại." });

            return Redirect(originalUrl);
        }
    }
}