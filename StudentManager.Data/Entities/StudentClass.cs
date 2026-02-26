using System.Collections.ObjectModel;

namespace StudentManager.Data.Entities
{
    public class StudentClass
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public virtual ICollection<Student> Students { get; set; } = new Collection<Student>();
    }
}
