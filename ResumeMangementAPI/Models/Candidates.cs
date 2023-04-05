namespace ResumeManagementAPI.Models
{
    public class Candidates
    {
        public int Id { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Coverletter { get; set; }
        public string ResumeUrl { get; set; }
        public int JobId { get;set; }
        public Jobs Job { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool isActive { get; set; } = true;
    }
}
