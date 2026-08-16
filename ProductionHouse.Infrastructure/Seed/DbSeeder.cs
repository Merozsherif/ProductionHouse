using Microsoft.AspNetCore.Identity;
using ProductionHouse.Core.Entities;
using ProductionHouse.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionHouse.Infrastructure.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(AppDbContext context)
        {
            if (context.Admins.Any())
                return;


            var hasher = new PasswordHasher<Admin>();


            var admin = new Admin
            {
                Name = "admin",
                Email = "admin@starmedia.com",
                Role = "Admin",
            };

            admin.PasswordHash = hasher.HashPassword(admin, "Admin@123");

            context.Admins.Add(admin);

            await context.SaveChangesAsync();
           

        }
    }
}
