using ResumeManagementAPI.Models;

namespace ResumeManagementAPI.DTO.Job
{
    public class JobUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public JobLevel level { get; set; }
        public int CompanyId { get; set; }
    }
}
