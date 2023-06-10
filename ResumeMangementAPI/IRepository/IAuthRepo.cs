using ResumeManagementAPI.DTO;
using ResumeManagementAPI.Models;

namespace ResumeManagementAPI.IRepository
{
    public interface IAuthRepo
    {
        Task<(int, string)> Registration(RegisterDTO dTO, string role);
        Task<TokenDTO> Login(LoginDTO loginDTO);
        Task<TokenDTO> GetRefreshToken(TokenRefresh requestDTO);

    }
}
