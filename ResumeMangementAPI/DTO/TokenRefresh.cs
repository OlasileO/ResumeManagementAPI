using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ResumeManagementAPI.DTO
{
    public class TokenRefresh
    {
       
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
