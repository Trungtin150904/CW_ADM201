using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManager.Common.DTOs;
using StudentManager.Data;
using StudentManager.Data.Entities;
using StudentManager.MVC.Models;

namespace StudentManager.MVC.Controllers
{
    public class MajorsController : Controller
    {
        private readonly StudentManagerDbContext _context;

        public MajorsController(StudentManagerDbContext context)
        {
            _context = context;
        }

        // GET: Majors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Majors.ToListAsync());
        }

        // GET: Majors/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = await _context.Majors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (major == null)
            {
                return NotFound();
            }

            return View(major);
        }

        // GET: Majors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Majors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,MajorCode")] MajorDTO majorDTO)
        //public async Task<IActionResult> Create([Bind("Id,Name,Description,MajorCode")] MajorDTO majorDTO)
        {
            if (ModelState.IsValid)
            {
                //major.Id = Guid.NewGuid();
                //_context.Add(major);
                var newMajor = new Major
                {
                    //Id = Guid.NewGuid(),
                    Name = majorDTO.Name.Trim(),
                    Description = majorDTO.Description?.Trim(),
                    MajorCode = majorDTO.MajorCode?.Trim(),
                };
                await _context.Majors.AddAsync(newMajor);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Create));
            }
            //return View();
            return View(majorDTO);
        }

        // GET: Majors/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var major = await _context.Majors.FindAsync(id);
            var majorDTO = await _context.Majors
                .Where(m => m.Id == id)
                .Select(m => new MajorDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    MajorCode = m.MajorCode,
                })
                .SingleOrDefaultAsync();
            if (majorDTO == null)
            {
                return NotFound();
            }
            //return View(major);
            return View(nameof(Create), majorDTO);
        }

        // POST: Majors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,MajorCode")] MajorDTO majorDTO)
        {
            if (id != majorDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(major);
                    //var major = _context.Majors.FindAsync(majorDTO.Id);
                    var major = await _context.Majors
                        .Where(m => m.Id == id)
                        .SingleOrDefaultAsync();
                    if(major != null)
                    {
                        major.Name = majorDTO.Name.Trim();
                        major.Description = majorDTO.Description?.Trim();
                        major.MajorCode = majorDTO.MajorCode?.Trim();
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MajorExists(majorDTO.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(nameof(Create), majorDTO);
        }

        // GET: Majors/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = await _context.Majors
                .Where(m => m.Id.Equals(id))
                .Select(m => new MajorVM
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    MajorCode = m.MajorCode,
                })
                .SingleOrDefaultAsync();
            if (major == null)
            {
                return NotFound();
            }

            return PartialView(major);
            return View(major);
        }

        // POST: Majors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            bool isOK = false;
            string message = string.Empty;
            try
            {
                var major = await _context.Majors.FindAsync(id);
                if (major != null)
                {
                    _context.Majors.Remove(major);
                }

                await _context.SaveChangesAsync();
                isOK = true;
                message = "Xoa thanh cong";
            }
            catch (Exception ex)
            {
                message = "Loi thuc thi " + ex.Message;
                //throw;
            }
            
            return Json(new {isOK, message});

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Reload()
        {
            return ViewComponent("Major");
        }

        private bool MajorExists(Guid id)
        {
            return _context.Majors.Any(e => e.Id == id);
        }
    }
}
