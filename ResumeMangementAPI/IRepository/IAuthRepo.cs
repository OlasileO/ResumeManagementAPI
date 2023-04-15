using ResumeManagementAPI.DTO;

namespace ResumeManagementAPI.IRepository
{
    public interface IAuthRepo
    {
        Task<(int, string)> Registration(RegisterDTO dTO, string role);
        Task<TokenDTO> Login(LoginDTO loginDTO);
        Task<TokenDTO> GetRefreshToken(TokenRequestDTO requestDTO);


    }
}
