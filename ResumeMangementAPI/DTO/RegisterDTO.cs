using System.ComponentModel.DataAnnotations;

namespace ResumeManagementAPI.DTO
{
    public class RegisterDTO
    {
       [Required]
       public string UserName { get; set; }
        [Required]
        public string FristName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
