using ResumeManagementAPI.Models;

namespace ResumeManagementAPI.DTO
{
    public class CreateCompanyDto
    {
        
        public string Name { get; set; }
        public CompanySize Size { get; set; }
    }
}
