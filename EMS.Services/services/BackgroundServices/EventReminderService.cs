using EMS.Core.Entities.Idenitty;
using EMS.Core.Services.Interfaces;
using EMS.Repository.Data.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.services.BackgroundServices
{

    public class EventReminderService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public EventReminderService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await SendReminders();
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }

        private async Task SendReminders()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var tomorrow = DateTime.UtcNow.AddHours(24);

            var events = await context.Events
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.User)
                .Where(e =>
                    e.DateTime <= tomorrow &&
                    e.DateTime > DateTime.UtcNow &&
                    !e.ReminderSent)
                .ToListAsync();

            foreach (var ev in events)
            {
                foreach (var registration in ev.Registrations
                    .Where(r => r.PaymentStatus == PaymentStatus.Success))
                {
                    string body = $@"
                    <h2>Reminder: {ev.Title}</h2>
                    <p>Date: {ev.DateTime}</p>
                    <p>Location: {ev.Location}</p>
                    <p>Don't forget your event!</p>";

                    await emailService.SendEmailAsync(
                        registration.User.Email,
                        "Event Reminder",
                        body);
                }

                ev.ReminderSent = true;
            }

            await context.SaveChangesAsync();
        }
    }


}
