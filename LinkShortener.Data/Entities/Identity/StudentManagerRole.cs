using Microsoft.AspNetCore.Identity;

namespace LinkShortener.Data.Entities.Identity
{
    public class StudentManagerRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
