using StudentManager.Common.Enums;
using System.ComponentModel;

namespace StudentManager.Data.Entities
{
    public class Student
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FullName { get; set; } = string.Empty;

        //public int Gender { get; set; }
        public GenderEnum Gender { get; set; }
        public HobbyEnum Hobby { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Description("Duong dan hinh anh dai dien")]
        public string? Avatar { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public Guid? MajorId { get; set; }
        public virtual Major Major { get; set; } = null!;

        public Guid StudentClassId { get; set; }
        public virtual StudentClass StudentClass { get; set; } = null!;
    }
}
