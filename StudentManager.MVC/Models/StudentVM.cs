using Microsoft.AspNetCore.Mvc;
using StudentManager.Common.Contants;
using StudentManager.Common.DTOs;
using StudentManager.Common.Enums;
using StudentManager.Data.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentManager.MVC.Models
{
    [Bind("Id,FullName,Avatar,Gender,Hobby, DateOfBirth,Phone,Email,Address,MajorId, StudentClassId")]
    public class StudentVM
    {
        public StudentVM()
        {

        }
        public StudentVM(StudentDTO student)
        {
            Id = student.Id;
            Major = student.Major;
            Address = student.Address;
            FullName = student.FullName;
            DateOfBirth = student.DateOfBirth;
            Email = student.Email;
            Gender = student.Gender;
            Hobby = student.Hobby;
            MajorId = student.MajorId;
            Phone = student.Phone;
            StudentClassId = student.StudentClassId;
        }
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(MaxLengths.FULL_NAME)]
        public string FullName { get; set; } = string.Empty;

        //public int Gender { get; set; }
        public GenderEnum Gender { get; set; }
        public HobbyEnum Hobby { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Description("Duong dan hinh anh dai dien")]
        //[MaxLength(MaxLengths.AVATAR)]
        public IFormFile? Avatar { get; set; }
        public string? AvatarPath { get; set; }

        [MaxLength(MaxLengths.PHONE)]
        public string? Phone { get; set; }

        [MaxLength(MaxLengths.EMAIL)]
        public string? Email { get; set; }
        [MaxLength(MaxLengths.ADDRESS)]
        public string? Address { get; set; }

        public Guid? MajorId { get; set; }
        public string? Major { get; set; }

        public Guid StudentClassId { get; set; }
        public string? StudentClass { get; set; }
    }
}
