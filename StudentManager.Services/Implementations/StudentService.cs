using Microsoft.EntityFrameworkCore;
using StudentManager.Common.DTOs;
using StudentManager.Data;
using StudentManager.Data.Entities;
using StudentManager.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentManager.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly StudentManagerDbContext _context;
        public StudentService(StudentManagerDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Create(StudentDTO dtoStudent)
        {
            //throw new NotImplementedException();
            var isOK = false;
            try
            {
                var newStudent = new Student
                {
                    FullName = dtoStudent.FullName.Trim(),
                    DateOfBirth = dtoStudent.DateOfBirth,
                    Address = dtoStudent.Address?.Trim(),
                    Email = dtoStudent.Email?.Trim(),
                    Phone = dtoStudent.Phone?.Trim(),
                    //Gender = (int)dtoStudent.Gender,
                    Gender = dtoStudent.Gender,
                    Hobby = dtoStudent.Hobby,
                    MajorId = dtoStudent.MajorId,
                    StudentClassId = dtoStudent.StudentClassId,
                    Avatar = dtoStudent.AvatarPath,
                };
                await _context.Students.AddAsync(newStudent);

                //if (dtoStudent.Avatar != null && dtoStudent.Avatar.Length > 0)
                //{
                //    var file = dtoStudent.Avatar;
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
                //            newStudent.Avatar = fileName;
                //        }
                //    }
                //}
                await _context.SaveChangesAsync();
                isOK = true;
            }
            catch (Exception ex)
            {

            }
            return isOK;
        }

        public async Task<bool> Delete(Guid idStudent)
        {
            var isOK = false;
            try
            {
                var student = await _context.Students.FindAsync(idStudent);
                if (student != null)
                {
                    _context.Students.Remove(student);
                }

                await _context.SaveChangesAsync();
                isOK = true;
            }
            catch (Exception ex)
            {

            }

            return isOK;
        }

        public async Task<StudentDTO[]?> GetAll()
        {
            //throw new NotImplementedException();
            try
            {
                var student = await _context.Students
                    .Include(s => s.Major)
                    .Select(s => new StudentDTO
                    {
                        Id = s.Id,
                        Address = s.Address,
                        //AvatarPath = s.Avatar,
                        DateOfBirth = s.DateOfBirth,
                        Email = s.Email,
                        FullName = s.FullName,
                        Phone = s.Phone,
                        Gender = s.Gender,
                        Hobby = s.Hobby,
                        MajorId = s.MajorId,
                        Major = s.Major.Name,
                        StudentClassId = s.StudentClassId,
                    })
                    .ToArrayAsync();
                return student;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<StudentDTO?> GetById(Guid idStudent)
        {
            //throw new NotImplementedException();
            try
            {
                var student = await _context.Students
                    .Where(s => s.Id == idStudent)
                    .Select(s => new StudentDTO
                    {
                        Id = s.Id,
                        Address = s.Address,
                        //AvatarPath = s.Avatar,
                        DateOfBirth = s.DateOfBirth,
                        Email = s.Email,
                        FullName = s.FullName,
                        Phone = s.Phone,
                        Gender = s.Gender,
                        Hobby = s.Hobby,
                        MajorId = s.MajorId,
                        StudentClassId = s.StudentClassId,
                    })
                    .SingleOrDefaultAsync();
                return student;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<StudentDTO?> GetByIdWithMajor(Guid idStudent)
        {
            //throw new NotImplementedException();
            try
            {
                var student = await _context.Students
                    .Where(s => s.Id == idStudent)
                    .Include(s => s.Major)
                    .Select(s => new StudentDTO
                    {
                        Id = s.Id,
                        Address = s.Address,
                        //AvatarPath = s.Avatar,
                        DateOfBirth = s.DateOfBirth,
                        Email = s.Email,
                        FullName = s.FullName,
                        Phone = s.Phone,
                        Gender = s.Gender,
                        Hobby = s.Hobby,
                        MajorId = s.MajorId,
                        Major = s.Major.Name,
                        StudentClassId = s.StudentClassId,
                    })
                    .SingleOrDefaultAsync();
                return student;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<bool> Update(StudentDTO studentDTO)
        {
            //throw new NotImplementedException();
            var isOK = false;
            try
            {
                var student = await _context.Students.FindAsync(studentDTO.Id);
                if (student != null)
                {
                    student.FullName = studentDTO.FullName.Trim();
                    student.Address = studentDTO.Address?.Trim();
                    student.Phone = studentDTO.Phone?.Trim();
                    student.Email = studentDTO.Email?.Trim();
                    student.DateOfBirth = studentDTO.DateOfBirth;
                    student.Gender = studentDTO.Gender;
                    student.Hobby = studentDTO.Hobby;
                    student.MajorId = studentDTO.MajorId;
                    student.StudentClassId = studentDTO.StudentClassId;

                    //_context.Update(student);
                    await _context.SaveChangesAsync();
                    isOK = true;
                }
            }
            catch (Exception ex)
            {

            }
            return isOK;
        }



    }
}
