namespace ResumeManagementAPI.DTO
{
    public class TokenDTO
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
