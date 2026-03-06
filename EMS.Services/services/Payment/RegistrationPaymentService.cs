using EMS.Core.Repository.Interfaces;
using EMS.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.services.Payment
{
    public class RegistrationPaymentService
    : StripePaymentService<Registration>
    {
        public RegistrationPaymentService(IConfiguration config, AppDbContext context)
            : base(config, context)
        {
        }

        protected override async Task<Registration> GetEntityAsync(int id)
        {
            return await _context.Registrations
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        protected override string GetPaymentIntentId(Registration reg)
            => reg.PaymentIntentId;

        protected override async Task SavePaymentIntentIdAsync(int id, string? paymentIntentId, string? clientSecret)
        {
            var registration = await _context.Registrations.FindAsync(id);
            registration.PaymentIntentId = paymentIntentId;
            registration.ClientSecret = clientSecret;
        }

        protected override async Task<decimal> GetAmountAsync(int registrationId)
        {
            var registration = await _context.Registrations
                .FirstOrDefaultAsync(r => r.Id == registrationId);

            if (registration == null)
                throw new Exception("Registration not found");

            return registration.TotalPrice;
        }


        protected override async Task<Registration> FindByIntent(string paymentIntentId)
        {
            return await _context.Registrations
                .FirstOrDefaultAsync(r => r.PaymentIntentId == paymentIntentId);
        }

        protected override Task UpdateStatusAsync(Registration reg, bool succeeded)
        {
            reg.PaymentStatus = succeeded ? PaymentStatus.Success : PaymentStatus.Failed;
            return Task.CompletedTask;
        }
    }

}
