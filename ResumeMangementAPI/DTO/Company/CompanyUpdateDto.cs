using ResumeManagementAPI.Models;

namespace ResumeManagementAPI.DTO.Company
{
    public class CompanyUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CompanySize Size { get; set; }

    }
}
