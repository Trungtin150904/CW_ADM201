using Microsoft.EntityFrameworkCore;
using StudentManager.Common.DTOs;
using StudentManager.Data;
using StudentManager.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManager.Services.Implementations
{
    public class MajorService : IMajorService
    {
        private readonly StudentManagerDbContext _context;
        public MajorService(StudentManagerDbContext context)
        {
            _context = context;
        }
        public async Task<MajorDTO[]?> GetAll()
        {
            try
            {
                var majors = await _context.Majors
                        .Select(x => new MajorDTO
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Description = x.Description,
                            MajorCode = x.MajorCode,
                        })
                        .ToArrayAsync();
                return majors;
            }
            catch (Exception)
            {

            }
            return null;
        }


    }
}
