using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.DTOs.Identity
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "DisplayName is required !!")]
        public string DisplayName { get; set; }
        [Required(ErrorMessage = "Email is required !!")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required !!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is required !!")]
        public string Role { get; set; }
    }
}
