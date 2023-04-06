using ResumeManagementAPI.Models;

namespace ResumeManagementAPI.DTO
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CompanySize Size { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
