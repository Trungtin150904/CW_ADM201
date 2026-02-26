using Microsoft.EntityFrameworkCore;
using StudentManager.Common.DTOs;
using StudentManager.Data;
using StudentManager.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManager.Services.Implementations
{
    public class StudentClassService : IStudentClassService
    {
        private readonly StudentManagerDbContext _context;
        public StudentClassService(StudentManagerDbContext context)
        {
            _context = context;
        }
        public async Task<StudentClassDTO[]?> GetAll()
        {
            try
            {
                var studentClassDTOs = await _context.StudentClasses
                    .Select(sc => new StudentClassDTO
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                        Description = sc.Description,
                    })
                    .ToArrayAsync();
                return studentClassDTOs;
            }
            catch (Exception)
            {
            }
            return null;
        }


    }
}
