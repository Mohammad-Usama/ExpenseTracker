using ExpenseTracker.Enums;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ExpenseTracker.Context
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            try
            {
                // Ensure that the database exists and all pending migrations are applied.
                context.Database.Migrate();

                // Create roles
                string[] roles = new string[] { Roles.Admin.ToString(), Roles.User.ToString() };
                foreach (string role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                string adminEmailAddress = "admin@gmail.com";
                string adminPassword = "Admin@123";
                string userEmailAddress = "aninda@gmail.com";
                string userPassword = "Aninda@123";

                // Create admin user
                var adminUser = await userManager.FindByEmailAsync(adminEmailAddress);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser()
                    {
                        Email = adminEmailAddress,
                        UserName = adminEmailAddress,
                        FirstName = "Admin",
                        LastName = "Admin",
                    };

                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        // Ensure admin privileges
                        ApplicationUser admin = await userManager.FindByEmailAsync(adminEmailAddress);
                        await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
                    }
                }

                var user = await userManager.FindByEmailAsync(userEmailAddress);
                if (user == null)
                {
                    user = new ApplicationUser()
                    {
                        Email = userEmailAddress,
                        UserName = userEmailAddress,
                        FirstName = "Aninda",
                        LastName = "Saha"
                    };

                    var result = await userManager.CreateAsync(user, userPassword);
                    if (result.Succeeded)
                    {
                        // Ensure doctor privileges
                        ApplicationUser user1 = await userManager.FindByEmailAsync(adminEmailAddress);
                        await userManager.AddToRoleAsync(user1, Roles.User.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
