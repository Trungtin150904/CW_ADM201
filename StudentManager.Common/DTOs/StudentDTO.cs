using StudentManager.Common.Contants;
using StudentManager.Common.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentManager.Common.DTOs
{
    public class StudentDTO
    {
        public Guid Id { get; set; }

        [MaxLength(MaxLengths.FULL_NAME)]
        public string FullName { get; set; } = string.Empty;

        public GenderEnum Gender { get; set; }
        public HobbyEnum Hobby { get; set; }
        public DateTime DateOfBirth { get; set; }

        //[Description("Duong dan hinh anh dai dien")]
        ////[MaxLength(MaxLengths.AVATAR)]
        //public IFormFile? Avatar { get; set; }
        public string? AvatarPath { get; set; }

        [MaxLength(15)]
        public string? Phone { get; set; }
        [MaxLength(250)]
        public string? Email { get; set; }
        [MaxLength(500)]
        public string? Address { get; set; }

        public Guid? MajorId { get; set; }
        public string? Major { get; set; }

        public Guid StudentClassId { get; set; }
        public string? StudentClass { get; set; }

    }
}
