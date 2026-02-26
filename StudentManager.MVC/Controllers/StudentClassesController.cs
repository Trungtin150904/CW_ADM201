using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManager.Data;
using StudentManager.Data.Entities;
using StudentManager.MVC.Models;

namespace StudentManager.MVC.Controllers
{
    public class StudentClassesController : Controller
    {
        private readonly StudentManagerDbContext _context;

        public StudentClassesController(StudentManagerDbContext context)
        {
            _context = context;
        }

        // GET: StudentClasses
        public IActionResult Index()
        {
            //return View(await _context.StudentClasses.ToListAsync());
            return View();
        }

        // GET: StudentClasses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentClass = await _context.StudentClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentClass == null)
            {
                return NotFound();
            }

            return View(studentClass);
        }

        // GET: StudentClasses/Create
        public IActionResult Create()
        {
            return PartialView();
            //return View();
        }

        // POST: StudentClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentClassVM studentClassVM)
        {
            if (ModelState.IsValid)
            {
                var newStudentClass = new StudentClass
                {
                   Name = studentClassVM.Name.Trim(), 
                   Description = studentClassVM.Description?.Trim(),
                };
                await _context.StudentClasses.AddAsync(newStudentClass);
                //studentClass.Id = Guid.NewGuid();
                //_context.Add(studentClass);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return Json(new { isOK = true, message = "Thanh cong" });
            }
            return PartialView(studentClassVM);
        }

        // GET: StudentClasses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var studentClass = await _context.StudentClasses.FindAsync(id);
            var studentClassVM = await _context.StudentClasses
                .Select(ss => new StudentClassVM
                {
                    Id = ss.Id,
                    Name = ss.Name,
                    Description = ss.Description,
                })
                .SingleOrDefaultAsync();
            if (studentClassVM == null)
            {
                return NotFound();
            }
            return PartialView(nameof(Create), studentClassVM);
        }

        // POST: StudentClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StudentClassVM studentClassVM)
        {
            if (id != studentClassVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(studentClass);
                    var studentClass = await _context.StudentClasses
                        .Where(ss => ss.Id == id)
                        .SingleOrDefaultAsync();
                    if (studentClass == null)
                    {
                        return BadRequest();
                    }
                    studentClass.Name = studentClassVM.Name.Trim();
                    studentClass.Description = studentClassVM.Description?.Trim();
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentClassExists(studentClassVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return PartialView(nameof(Create), studentClassVM);
        }

        // GET: StudentClasses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentClass = await _context.StudentClasses
                .Select(sc => new StudentClassVM
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    Description = sc.Description,
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentClass == null)
            {
                return NotFound();
            }

            return PartialView(studentClass);
        }

        // POST: StudentClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            bool isOK = false;
            string message = string.Empty;
            try
            {
                var studentClass = await _context.StudentClasses.FindAsync(id);
                if (studentClass != null)
                {
                    _context.StudentClasses.Remove(studentClass);
                }

                await _context.SaveChangesAsync();
                isOK = true;
                message = "Xoa1 thanh cong roi nha may cha noi";
            }
            catch (Exception ex)
            {
                message = "Loi thuc thi " + ex.Message;
            }
            
            //return RedirectToAction(nameof(Index));
            //return Json( new {isOK =  isOK, message = message});
            return Json( new { isOK, message});
        }

        public IActionResult Refresh()
        {
            return ViewComponent("StudentClassList");
        }

        private bool StudentClassExists(Guid id)
        {
            return _context.StudentClasses.Any(e => e.Id == id);
        }
    }
}
