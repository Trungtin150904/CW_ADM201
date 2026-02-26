using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManager.Common.DTOs;
using StudentManager.Data;

namespace StudentManager.MVC.ViewComponents
{
    public class StudentsViewComponent: ViewComponent
    {
        private readonly StudentManagerDbContext _context;

        public StudentsViewComponent(StudentManagerDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var students = await _context.Students
                .Include(s => s.Major)
                .Select(s => new StudentDTO
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    Phone = s.Phone,
                    Email = s.Email,
                    Address = s.Address,
                    DateOfBirth = s.DateOfBirth,
                    Gender = s.Gender,
                    MajorId = s.MajorId,
                    Major = s.Major.Name,
                })
                .ToListAsync();
            return View(students); // Trả về View Default.cshtml
        }
    }
}
