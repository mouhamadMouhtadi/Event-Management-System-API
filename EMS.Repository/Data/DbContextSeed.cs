using EMS.Core.Entities;
using EMS.Core.Entities.Idenitty;
using EMS.Repository.Data.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EMS.Repository.Data
{
    public class DbContextSeed
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, AppDbContext context)
        {
            // 1️⃣ Seed Identity Users
            if (!userManager.Users.Any())
            {
                var usersData = File.ReadAllText(@"..\EMS.Repository\Data\DataSeed\Users.json");
                var users = JsonSerializer.Deserialize<List<AppUser>>(usersData);

                if (users is not null && users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        // Add default password if needed
                        await userManager.CreateAsync(user, "P@ssw0rd");
                    }
                }
            }

            // 2️⃣ Seed Categories
            if (!context.Categories.Any())
            {
                var categoriesData = File.ReadAllText(@"..\EMS.Repository\Data\DataSeed\Categories.json");
                var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);

                if (categories is not null && categories.Count > 0)
                {
                    await context.Categories.AddRangeAsync(categories);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
