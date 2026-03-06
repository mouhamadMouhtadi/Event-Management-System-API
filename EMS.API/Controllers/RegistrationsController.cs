using EMS.Core.DTOs.Registration;
using EMS.Core.Entities.Payment;
using EMS.Core.Services.Interfaces;
using EMS.Services.services.Registrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Organizer,Attendee")]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationsService _serivces;

        public RegistrationsController(IRegistrationsService serivces)
        {
            _serivces = serivces;
        }
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateRegistrationRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _serivces.RegisterAsync(userId, request.EventId);

            if (!result) return BadRequest("Registration failed.");

            return Ok("Registered successfully.");
        }
        [HttpDelete("unregister/{eventId}")]
        public async Task<IActionResult> UnRegister(int eventId )
        {
            var userId = GetUserId();
            var result = await _serivces.UnRegisterAsync(userId, eventId);
            if (!result)
                return BadRequest("Not registered for this event.");
            return Ok("Unregistered successfully.");
        }
        [HttpGet("my-registrations")]
        public async Task<IActionResult> MyRegistrations()
        {
            var userId = GetUserId();
            var result = await _serivces.GetUserRegistrationAsync(userId);
            return Ok(result);
        }
    }
}
