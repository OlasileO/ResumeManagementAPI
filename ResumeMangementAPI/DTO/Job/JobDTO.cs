using ResumeManagementAPI.DTO.Company;
using ResumeManagementAPI.Models;

namespace ResumeManagementAPI.DTO.Job
{
    public class JobDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public JobLevel level { get; set; }
        public int CompanyId { get; set; }
        public CompanyDto Company { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
