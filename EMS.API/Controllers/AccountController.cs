using EMS.Core.DTOs.Identity;
using EMS.Core.Entities.Idenitty;
using EMS.Core.Services.Interfaces;
using EMS.Services.services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountController(IUserService userService,UserManager<AppUser> userManager,ITokenService tokenService)
        {
            _userService = userService;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login (LoginDto login)
        {
            var user = await _userService.LoginAsync(login);
            if ( user == null) return Unauthorized();
            return Ok(user);
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto> > Register (RegisterDto register)
        {
            var user  = await _userService.RegisterAsync(register);
            if ( user == null) return BadRequest();
            return Ok(user);
        }
        [HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return BadRequest();
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null) return BadRequest();
            return Ok(new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }
    }
}
