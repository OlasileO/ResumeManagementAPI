using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeManagementAPI.DTO;
using ResumeManagementAPI.IRepository;
using ResumeManagementAPI.Models;

namespace ResumeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthRepo _authRepo;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly UserManager<AppUser> _userManager;

        public AuthenticationController(IAuthRepo authRepo, 
            ILogger<AuthenticationController> logger, 
            UserManager<AppUser> userManager)
        {
            _authRepo = authRepo;
            _logger = logger;
            _userManager = userManager;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
                var result = await _authRepo.Login(model);
                if (result.StatusCode == 0)
                    return BadRequest(result.StatusMessage);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
           
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var(status, message) = await _authRepo.Registration(model,UserRoles.User);
                if (status == 0)
                    return BadRequest(message);
                return CreatedAtAction(nameof(Register), model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        [Route("refreshToken")]
        public async Task<IActionResult> RefreshToken(TokenRequestDTO model)
        {
            try
            {
                if (model != null)
                    return BadRequest("Invalid BadRequest");
                var result = await _authRepo.GetRefreshToken(model);
                if (result.StatusCode == 0)
                    return BadRequest(result.StatusMessage);
                return Ok(result);
                
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //[Authorize]
        //[HttpPost]
        //[Route("revoke/username")]
        //public async Task<IActionResult> Revoke(string username)
        //{
        //    var user =await  _userManager.FindByNameAsync(username);
        //    if (user == null)
        //        return BadRequest("Invalid UserName");
        //    user.RefreshToken = null;
        //    await _userManager.UpdateAsync(user);
        //    return Ok("Success");
        //}

        //[Authorize]
        //[HttpPost]
        //[Route("revokeAll")]
        //public async Task<IActionResult> RevokeAll()
        //{
        //    var users = await _userManager.Users.ToListAsync();
        //    foreach (var user in users)
        //    {
        //        user.RefreshToken = null;
        //        await _userManager.UpdateAsync(user);
        //    }
        //    return Ok("Success");
        //}
    }
}
