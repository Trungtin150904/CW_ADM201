using Microsoft.AspNetCore.Identity;

namespace LinkShortener.Data.Entities.Identity
{
    public class StudentManagerUser : IdentityUser
    {
        public string? Avatar { get; set; }
        public string? FullName { get; set; }
    }
}
