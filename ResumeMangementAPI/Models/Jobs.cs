namespace ResumeManagementAPI.Models
{
    public class Jobs
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public JobLevel level { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool isActive { get; set; } = true;
        public ICollection<Candidates> Candidates { get; set; } 
    }
}
