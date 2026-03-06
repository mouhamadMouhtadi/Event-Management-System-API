using EMS.Core.DTOs.Registration;
using EMS.Core.Entities;
using EMS.Core.Entities.Idenitty;
using EMS.Core.Services.Interfaces;
using EMS.Repository.Data.Contexts;
using EMS.Services.services.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EMS.Services.services.Registrations
{
    public class RegistrationsService : IRegistrationsService
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;

        public RegistrationsService(AppDbContext context , IEmailService emailService, UserManager<AppUser> userManager)
        {
            _context = context;
            _emailService = emailService;
            _userManager = userManager;
        }
        public async Task<bool> RegisterAsync(string userId, int eventId)
        {
            using var transaction = await _context.Database
                .BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

            var ev = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null || ev.Status != EventStatus.Scheduled)
                return false;

            if (ev.OrganizerId == userId)
                return false;

            var alreadyRegistered = await _context.Registrations
                .AnyAsync(r => r.UserId == userId && r.EventId == eventId);

            if (alreadyRegistered)
                return false;

            var count = await _context.Registrations
                .CountAsync(r => r.EventId == eventId);

            if (count >= ev.MaxAttendees)
                return false;

            var registration = new Registration
            {
                UserId = userId,
                EventId = eventId,
                TotalPrice = ev.Price,
                PaymentStatus = PaymentStatus.Pending

            };

            if (ev.Price == 0)
            {
                registration.PaymentStatus = PaymentStatus.Success;
            }

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                string htmlBody = $@"
            <h2>You have successfully registered for the event: {ev.Title}</h2>
            <p>Date: {ev.DateTime}</p>
            <p>Location: {ev.Location}</p>";

                await _emailService.SendEmailAsync(
                    user.Email,
                    "Event Registration Confirmed",
                    htmlBody
                );
            }

            return true;
        }


        public async Task<bool> UnRegisterAsync(string UserId, int EventId)
        {
            var registration = await _context.Registrations
                .FirstOrDefaultAsync(r => r.UserId == UserId && r.EventId == EventId);

            if (registration == null) return false;

            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();
            return true;

        }
        public Task<List<RegistrationResponse>> GetUserRegistrationAsync(string UserId)
        {
            return _context.Registrations
                .Where(r => r.UserId == UserId)
                .Select(r => new RegistrationResponse
                {
                    Id = r.Id,
                    EventId = r.EventId,
                    EventTitle = r.Event.Title,
                    PaymentStatus = r.PaymentStatus.ToString()
                })
                .ToListAsync();
        }

        public async Task<bool> UpdatePaymentStatusAsync(int registrationId, string newStatus)
        {
            var registration = await _context.Registrations.FirstOrDefaultAsync(r => r.Id == registrationId);
            if ( registration == null) return false;
            if (!Enum.TryParse<PaymentStatus>(newStatus, true, out var statusEnum))
                return false;
            registration.PaymentStatus = statusEnum;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
