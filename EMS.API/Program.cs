using EMS.Core.Entities.Idenitty;
using EMS.Core.Entities.SMTP;
using EMS.Core.Mapping.Events;
using EMS.Core.Repository.Interfaces;
using EMS.Core.Services.Interfaces;
using EMS.Repository.Data;
using EMS.Repository.Data.Contexts;
using EMS.Repository.Identity.DataSeed;
using EMS.Repository.Repository;
using EMS.Services.services.BackgroundServices;
using EMS.Services.services.Email;
using EMS.Services.services.Events;
using EMS.Services.services.Payment;
using EMS.Services.services.Registrations;
using EMS.Services.services.Token;
using EMS.Services.services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EMS.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1️⃣ Controllers & Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 2️⃣ Unified DbContext (AppDbContext includes Identity)
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.Configure<EmailSettings>(
                builder.Configuration.GetSection("EmailSettings"));

            // 3️⃣ Identity + Roles + TokenProviders
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            // 4️⃣ Dependency Injection for Services
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITokenService, TokenServcie>();
            builder.Services.AddScoped<IRegistrationsService, RegistrationsService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IPaymentService<Registration>,RegistrationPaymentService>();
            builder.Services.AddHostedService<EventReminderService>();



            // 5️⃣ AutoMapper Profiles
            builder.Services.AddAutoMapper(m => m.AddProfile(new EventProfile()));

            // 6️⃣ JWT Authentication Config
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),

                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],

                    ValidateLifetime = true
                };
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // 7️⃣ Database migration + Seeding
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                await context.Database.MigrateAsync();

                await AppIdentityDbContextSeed.SeedRolesAsync(roleManager);  // seed roles first
                await DbContextSeed.SeedAsync(userManager,context);  // seed users
                await AppIdentityDbContextSeed.SeedAppUserAsync(userManager); 

            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during migration or seeding.");
            }

            // 8️⃣ Middlewares
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
