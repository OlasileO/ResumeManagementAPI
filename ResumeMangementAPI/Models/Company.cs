namespace ResumeManagementAPI.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CompanySize Size { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool isActive { get; set; } = true;
        public ICollection<Job> jobs { get; set; }
    }
}
