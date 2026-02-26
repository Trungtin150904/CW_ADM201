using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManager.Common.DTOs;
using StudentManager.Data;
using StudentManager.MVC.Models;

namespace StudentManager.MVC.ViewComponents
{
    public class StudentClassList : ViewComponent
    {
        private readonly StudentManagerDbContext _context;

        public StudentClassList(StudentManagerDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var students = await _context.StudentClasses
                .Select(s => new StudentClassVM
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                })
                .ToListAsync();
            return View(students); // Trả về View Default.cshtml
        }
    }
}
