using EMS.Core.DTOs.Identity;
using EMS.Core.Entities.Idenitty;
using EMS.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;


        public UserService(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }


        public async Task<UserDto> LoginAsync(LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user is null) return null;
            var result = await _signInManager.CheckPasswordSignInAsync(user,login.Password,false);
            if (!result.Succeeded) return null;
            return new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            // 1️⃣ تحقق من الإيميل إذا موجود
            if (await CheckIfEmailExist(registerDto.Email))
                return null;

           
            var user = new AppUser
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.Email.Split("@")[0]
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return null;

           
            var role = string.IsNullOrEmpty(registerDto.Role) ? "Attendee" : registerDto.Role;


            await _userManager.AddToRoleAsync(user, role);

       
            return new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
            };
        }


        public async Task<bool> CheckIfEmailExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
