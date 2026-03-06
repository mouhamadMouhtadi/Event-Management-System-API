using EMS.Core.Entities.Idenitty;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Repository.Identity.DataSeed
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new[] { "Admin", "Organizer", "Attendee" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        public static async Task SeedAppUserAsync(UserManager<AppUser> userManager)
        {

            if (!userManager.Users.Any())
            {
                var adminUser = new AppUser
                {
                    Email = "mhamadmouhtadi@gmail.com",
                    DisplayName = "Mhamad Mouhtadi",
                    UserName = "Mhamad.Mouhtadi",
                    Address = new Address
                    {
                        FName = "Mhamad",
                        LName = "Mouhtadi",
                        Street = "Main Road",
                        City = "Halba",
                        Country = "Lebanon"
                    }
                };

                await userManager.CreateAsync(adminUser, "P@ssw0rd");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
