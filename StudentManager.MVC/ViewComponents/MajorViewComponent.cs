using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManager.Common.DTOs;
using StudentManager.Data;

namespace StudentManager.MVC.ViewComponents
{
    public class MajorViewComponent : ViewComponent
    {
        private readonly StudentManagerDbContext _context;

        public MajorViewComponent(StudentManagerDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var majors = await _context.Majors
                .Select(m => new MajorDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    MajorCode = m.MajorCode,
                })
                .ToListAsync();
            return View(majors); // Trả về View Default.cshtml
        }


    }
}
