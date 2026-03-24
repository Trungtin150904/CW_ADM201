using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using LinkShortener.Data;
using LinkShortener.MVC.Models;

namespace LinkShortener.MVC.Controllers
{
    public class QRCodeController : Controller
    {
        private readonly LinkShortenerDbContext _context;
        public QRCodeController(LinkShortenerDbContext context)
        {
            _context = context;     
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(QRCodeVM model)
        {
            return View();
        } 

    }
}
