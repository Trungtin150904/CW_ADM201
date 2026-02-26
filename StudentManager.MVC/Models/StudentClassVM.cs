using Microsoft.AspNetCore.Mvc;

namespace StudentManager.MVC.Models
{
    [Bind("Id,Name,Description")]
    public class StudentClassVM
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
