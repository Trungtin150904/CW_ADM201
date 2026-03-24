using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LinkShortener.Services.Interfaces;

namespace LinkShortener.MVC.Controllers
{
    [Authorize] // Phải đăng nhập mới vào được
    public class ShortUrlController : Controller
    {
        private readonly IShortUrlService _service;

        public ShortUrlController(IShortUrlService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var list = await _service.GetAllAsync(baseUrl);
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string originalUrl)
        {
            if (string.IsNullOrWhiteSpace(originalUrl))
            {
                TempData["Error"] = "URL cannot be empty.";
                return RedirectToAction("Index");
            }
            try
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var result = await _service.CreateShortUrlAsync(originalUrl, baseUrl);
                TempData["ShortLink"]= result.ShortLink;
                TempData["Success"] = "Successfully created a shortened URL!";
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        // Chỉ Admin mới được xóa
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            TempData[result ? "Success" : "Error"] = result ? "Deletion successful!" : "URL not found.";
            return RedirectToAction("Index");
        }

        // Không cần Authorize — ai có link cũng redirect được
        [AllowAnonymous]
        [HttpGet("/g/{code}")]
        public async Task<IActionResult> RedirectToUrl(string code)
        {
            await _service.IncrementClickCountAsync(code);
            var originalUrl = await _service.GetOriginalUrlAsync(code);
            if (originalUrl == null) return NotFound("Short URL không tồn tại.");
            return Redirect(originalUrl);
        }
    }
}