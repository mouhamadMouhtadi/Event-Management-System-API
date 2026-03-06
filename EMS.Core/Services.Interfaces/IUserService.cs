using EMS.Core.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> LoginAsync(LoginDto login);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<bool> CheckIfEmailExist(string email);

    }
}
