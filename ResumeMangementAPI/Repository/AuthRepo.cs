using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ResumeManagementAPI.DTO;
using ResumeManagementAPI.IRepository;
using ResumeManagementAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ResumeManagementAPI.Repository
{
    public class AuthRepo: IAuthRepo
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole>_roleManager;
        private readonly IConfiguration _configuration;

        public AuthRepo(UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<TokenDTO> GetRefreshToken(TokenRequestDTO requestDTO)
        {

            TokenDTO _tokenDTO = new TokenDTO();
            var principal = GetPrincipalFromExpiredToken(requestDTO.Token);
            string username = principal.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != requestDTO.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                _tokenDTO.StatusCode = 0;
                _tokenDTO.StatusMessage = "Invalid access token or refresh token";
                return _tokenDTO;
            }

            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var newToken = GenerateToken(authClaims);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            _tokenDTO.StatusCode = 1;
            _tokenDTO.StatusMessage = "Success";
            _tokenDTO.Token = newToken;
            _tokenDTO.RefreshToken = newRefreshToken;
            return _tokenDTO;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"]));
            var _TokenExpiryTimeInHour = Convert.ToInt64(_configuration["JWTKey:TokenExpiryMinutes"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWTKey:ValidIssuer"],
                Audience = _configuration["JWTKey:ValidAudience"],
                Expires = DateTime.UtcNow.AddMinutes(_TokenExpiryTimeInHour),
                //Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(authClaims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"])),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public async Task<TokenDTO> Login(LoginDTO loginDTO)
        {
            TokenDTO _tokenDTO = new TokenDTO();
            var user = await _userManager.FindByNameAsync(loginDTO.Username);
            if (user is null)
            {
                _tokenDTO.StatusCode = 0;
                _tokenDTO.StatusMessage = "Invalid User";
                return _tokenDTO;
            }

            if (!await _userManager.CheckPasswordAsync(user,loginDTO.Password))
            {
                _tokenDTO.StatusCode = 0;
                _tokenDTO.StatusMessage = "Invalid Password";
                return _tokenDTO;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            };
            foreach (var userRole in userRoles )
            {
                authClaim.Add(new Claim(ClaimTypes.Role, userRole));
            }
            _tokenDTO.Token = GenerateToken(authClaim);
            _tokenDTO.RefreshToken = GenerateRefreshToken();
            _tokenDTO.StatusCode = 1;
            _tokenDTO.StatusMessage = "Success";

            var _RefreshTokenValidityInDays = Convert.ToInt64(_configuration["JWTKey:RefreshTokenValidityInDays"]);
            user.RefreshToken = _tokenDTO.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddHours(_RefreshTokenValidityInDays);
            await _userManager.UpdateAsync(user);

            return _tokenDTO;
        }

        public async Task<(int, string)> Registration(RegisterDTO dTO, string role)
        {
            var userExists = await _userManager.FindByEmailAsync(dTO.UserName);
            if (userExists != null)
                return (0, "User already exists");
            AppUser user = new()
            {
                Email = dTO.Email,
                UserName = dTO.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                FristName = dTO.FristName,
                LastName = dTO.LastName,
            };
            var createUser = await _userManager.CreateAsync(user,dTO.Password);
            if (!createUser.Succeeded)
                return (0, "User creation failed! Please check user details and try again");
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            if (await _roleManager.RoleExistsAsync(role))
                await _userManager.AddToRoleAsync(user, role);

            return (1, "User Successfully Created");
        }
    }
}
