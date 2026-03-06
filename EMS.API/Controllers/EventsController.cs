using EMS.Core.DTOs.Events;
using EMS.Core.Services.Interfaces;
using EMS.Core.Specifications.EventsSpec;
using EMS.Repository.Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.Claims;

namespace EMS.API.Controllers
{
    [Authorize(Roles ="Admin,Organizer")]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly AppDbContext _context;

        public EventsController(IEventService eventService ,AppDbContext context)
        {
            _eventService = eventService;
            _context = context;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllEvents([FromQuery] EventSpecParams productSpec)
        {
            var events = await _eventService.GetAllAsync(productSpec);
            return Ok(events);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var evt = await _eventService.GetByIdAsync(id);
            if (evt == null) return NotFound();
            return Ok(evt);
        }
        [Authorize(Roles ="Admin,Organizer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var organizerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (organizerId == null)
                return Unauthorized("Organizer ID missing from token.");


           
            var newEvent = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                DateTime = dto.StartDate,
                OrganizerId = organizerId, 
                Status = EventStatus.Scheduled,
                PaymentRequired = false,
                MaxAttendees = 100,
                Location = "Unknown",
                Price = dto.Price,
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEventById), new { id = newEvent.Id }, newEvent);
        }

        [Authorize(Roles = "Admin,Organizer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateEventRequest dto)
        {
            var eventEntity = await _eventService.GetByIdAsync(id);
            if (eventEntity == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (eventEntity.OrganizerId != userId && !isAdmin)
                return Forbid();
            var result = await _eventService.UpdateAsync(id, dto);
            return NoContent();
        }

        [Authorize(Roles = "Admin,Organizer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eventEntity = await _eventService.GetByIdAsync(id);
            if (eventEntity == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (eventEntity.OrganizerId != userId && !isAdmin)
                return Forbid(); // 403 Forbidden

            var result = await _eventService.DeleteAsync(id);
            return NoContent();
        }

    }
}
