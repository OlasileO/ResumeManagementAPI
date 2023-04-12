namespace ResumeManagementAPI.DTO.Candidate
{
    public class CandidateDTO
    {
        public int Id { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Coverletter { get; set; }
        public string ResumeUrl { get; set; }
        public int JobId { get; set; }
    }
}
