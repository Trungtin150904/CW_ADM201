using System.ComponentModel.DataAnnotations;

namespace StudentManager.MVC.Models
{
    public class MajorVM
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string? Description { get; set; }

        [MaxLength(15)]
        public string? MajorCode { get; set; }
    }
}
