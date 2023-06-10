using Microsoft.AspNetCore.Identity;

namespace ResumeManagementAPI.Models
{
    public class AppUser:IdentityUser
    {
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool isActive { get; set; } = true;
    }
}
