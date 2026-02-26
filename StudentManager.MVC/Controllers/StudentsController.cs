using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManager.Common.DTOs;
using StudentManager.Data;
using StudentManager.Data.Entities;
using StudentManager.Data.Interfaces;
using StudentManager.MVC.Helpers;
using StudentManager.MVC.Mappers;
using StudentManager.MVC.Models;
using System.Threading.Tasks;

namespace StudentManager.MVC.Controllers
{
    public class StudentsController : Controller
    {
        //private readonly StudentManagerDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IStudentService _serviceStudent;
        private readonly IMajorService _serviceMajor;
        private readonly IStudentClassService _srvStudentClass;
        public StudentsController(IWebHostEnvironment environment, IStudentService serviceStudent,
            IMajorService serviceMajor, IStudentClassService srvStudentClass)
        {
            //_context = context;
            _serviceStudent = serviceStudent;
            _serviceMajor = serviceMajor;
            _environment = environment;
            _srvStudentClass = srvStudentClass;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            //var studentManagerDbContext = _context.Students.Include(s => s.Major);
            //return View(await studentManagerDbContext.ToListAsync());
            var students = await _serviceStudent.GetAll();
            var studentVMs = students?.Select(s => new StudentVM(s)).ToArray();
            return View(studentVMs);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var student = await _context.Students
            //    .Include(s => s.Major)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var student = await _serviceStudent.GetById(id.Value);
            if (student == null)
            {
                return NotFound();
            }
            var studentVM = new StudentVM(student);
            //var studentVM = new StudentVM
            //{
            //    Id = student.Id,
            //    Major = student.Major,
            //    Address = student.Address,
            //    FullName = student.FullName,
            //    DateOfBirth = student.DateOfBirth,
            //    Email = student.Email,
            //    Gender = student.Gender,
            //    Hobby = student.Hobby,
            //    MajorId = student.MajorId,
            //    Phone = student.Phone,
            //    StudentClassId = student.StudentClassId
            //};
            return View(studentVM);
        }

        // GET: Students/Create
        public async Task<IActionResult> Create()
        {
            var majorDTOs = await _serviceMajor.GetAll();
            if (majorDTOs != null && majorDTOs.Length > 0)
            {
                ViewData["MajorId"] = new SelectList(majorDTOs, "Id", "Name");
            }

            var studentClassDTOs = await _srvStudentClass.GetAll();
            if (studentClassDTOs != null && studentClassDTOs.Length > 0)
            {
                ViewData["ClassId"] = new SelectList(studentClassDTOs, "Id", "Name");
            }
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentVM studentVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //student.Id = Guid.NewGuid();
                    //_context.Add(student);

                    //var newStudent = new Student
                    //{
                    //    FullName = studentVM.FullName.Trim(),
                    //    DateOfBirth = studentVM.DateOfBirth,
                    //    Address = studentVM.Address?.Trim(),
                    //    Email = studentVM.Email?.Trim(),
                    //    Phone = studentVM.Phone?.Trim(),
                    //    //Gender = (int)studentVM.Gender,
                    //    Gender = studentVM.Gender,
                    //    Hobby = studentVM.Hobby,
                    //    MajorId = studentVM.MajorId,
                    //    StudentClassId = studentVM.StudentClassId,
                    //};
                    //await _context.Students.AddAsync(newStudent);

                    //if (studentVM.Avatar != null && studentVM.Avatar.Length > 0)
                    //{
                    //    var file = studentVM.Avatar;
                    //    var extension = Path.GetExtension(file.FileName).ToLower();
                    //    var arrayAllowedFile = new string[] { ".jpg", ".jpeg", ".png" };
                    //    if (arrayAllowedFile.Contains(extension))
                    //    {
                    //        var allowedSize = 5 * 1024 * 1024;
                    //        if (file.Length < allowedSize)
                    //        {
                    //            var fileName = Guid.NewGuid().ToString() + extension;
                    //            var wwwRootFolder = _environment.WebRootPath;
                    //            var mediaFolder = Path.Combine(wwwRootFolder, "media", "images");
                    //            if (!Directory.Exists(mediaFolder))
                    //            {
                    //                Directory.CreateDirectory(mediaFolder);
                    //            }
                    //            var destinationFile = Path.Combine(mediaFolder, fileName);
                    //            using (var fileStream = new FileStream(destinationFile, FileMode.Create))
                    //            {
                    //                await file.CopyToAsync(fileStream);
                    //            }
                    //            studentVM.AvatarPath = fileName;
                    //        }
                    //    }
                    //}
                    //await _context.SaveChangesAsync();
                    var isSucceeded = await MediaHelper.SaveMedia(studentVM, _environment.WebRootPath);
                    //var studentDTO = new StudentDTO 
                    //{
                    //    Id = studentVM.Id,
                    //    Address = studentVM.Address,
                    //    DateOfBirth = studentVM.DateOfBirth,
                    //    Email = studentVM.Email,
                    //    FullName = studentVM.FullName,
                    //    Gender = studentVM.Gender,
                    //    Hobby = studentVM.Hobby,
                    //    MajorId = studentVM.MajorId,
                    //    Phone = studentVM.Phone,
                    //    StudentClassId = studentVM.StudentClassId,
                    //    AvatarPath = studentVM.AvatarPath,
                    //};
                    var studentDTO = StudentMapper.ToDTO(studentVM);
                    var isOK = await _serviceStudent.Create(studentDTO);
                }
                catch (Exception ex)
                {

                }

                return RedirectToAction(nameof(Index));
            }
            var majorDTOs = await _serviceMajor.GetAll();
            if (majorDTOs != null && majorDTOs.Length > 0)
            {
                ViewData["MajorId"] = new SelectList(majorDTOs, "Id", "Name", studentVM.MajorId);

            }

            var studentClassDTOs = await _srvStudentClass.GetAll();
            if (studentClassDTOs != null && studentClassDTOs.Length > 0)
            {
                ViewData["ClassId"] = new SelectList(studentClassDTOs, "Id", "Name", studentVM.StudentClassId);
            }

            return View(studentVM);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = await _serviceStudent.GetById(id.Value);

            //var student = await _context.Students
            //    .Select(s => new StudentVM
            //    {
            //        Id = s.Id,
            //        Address = s.Address,
            //        AvatarPath = s.Avatar,
            //        DateOfBirth = s.DateOfBirth,
            //        Email = s.Email,
            //        FullName = s.FullName,
            //        Phone = s.Phone,
            //        Gender = s.Gender,
            //        Hobby = s.Hobby,
            //        MajorId = s.MajorId,
            //        StudentClassId = s.StudentClassId,
            //    })
            //    .SingleOrDefaultAsync(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            var majorDTOs = await _serviceMajor.GetAll();
            if (majorDTOs != null && majorDTOs.Length > 0)
            {
                ViewData["MajorId"] = new SelectList(majorDTOs, "Id", "Name", student.MajorId);

            }

            var studentClassDTOs = await _srvStudentClass.GetAll();
            if (studentClassDTOs != null && studentClassDTOs.Length > 0)
            {
                ViewData["ClassId"] = new SelectList(studentClassDTOs, "Id", "Name", student.StudentClassId);
            }
            var studentVM = new StudentVM(student);
            return View(nameof(Create), studentVM);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StudentVM studentVM)
        {
            if (id != studentVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var studentDTO = new StudentDTO
                    {
                        Id = studentVM.Id,
                        Address = studentVM.Address,
                        DateOfBirth = studentVM.DateOfBirth,
                        Email = studentVM.Email,
                        FullName = studentVM.FullName,
                        Gender = studentVM.Gender,
                        Hobby = studentVM.Hobby,
                        MajorId = studentVM.MajorId,
                        Phone = studentVM.Phone,
                        StudentClassId = studentVM.StudentClassId,
                    };
                    var isOK = await _serviceStudent.Update(studentDTO);
                    //var student = await _context.Students.FindAsync(studentVM.Id);
                    //if (student != null)
                    //{
                    //    student.FullName = studentVM.FullName.Trim();
                    //    student.Address = studentVM.Address?.Trim();
                    //    student.Phone = studentVM.Phone?.Trim();
                    //    student.Email = studentVM.Email?.Trim();
                    //    student.DateOfBirth = studentVM.DateOfBirth;
                    //    student.Gender = studentVM.Gender;
                    //    student.Hobby = studentVM.Hobby;
                    //    student.MajorId = studentVM.MajorId;
                    //    student.StudentClassId = studentVM.StudentClassId;

                    //    //_context.Update(student);
                    //    await _context.SaveChangesAsync();
                    //}
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await StudentExists(studentVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Create));
            }
            var majorDTOs = await _serviceMajor.GetAll();
            if (majorDTOs != null && majorDTOs.Length > 0)
            {
                ViewData["MajorId"] = new SelectList(majorDTOs, "Id", "Name", studentVM.MajorId);

            }

            var studentClassDTOs = await _srvStudentClass.GetAll();
            if (studentClassDTOs != null && studentClassDTOs.Length > 0)
            {
                ViewData["ClassId"] = new SelectList(studentClassDTOs, "Id", "Name", studentVM.StudentClassId);
            }

            return View(studentVM);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var student = await _context.Students
            //    .Include(s => s.Major)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var studentDTO = await _serviceStudent.GetByIdWithMajor(id.Value);
            if (studentDTO == null)
            {
                return NotFound();
            }
            var studentVM = new StudentVM(studentDTO);
            return View(studentVM);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var isOK = await _serviceStudent.Delete(id);
            //var student = await _context.Students.FindAsync(id);
            //if (student != null)
            //{
            //    _context.Students.Remove(student);
            //}

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> StudentExists(Guid id)
        {
            var student = await _serviceStudent.GetById(id);

            //return _context.Students.Any(e => e.Id == id);
            return student != null;
        }
    }
}
