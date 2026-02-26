using System.Collections.ObjectModel;

namespace StudentManager.Data.Entities
{
    public class Major
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        //[MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? MajorCode { get; set; }

        public ICollection<Student> Students { get; set; } = new Collection<Student>();
    }
}
