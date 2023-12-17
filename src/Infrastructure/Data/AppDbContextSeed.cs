using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class AppDbContextSeed
    {
        public static async Task SeedAsync(AppDbContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            try
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("SurveyDesigner"));
                await roleManager.CreateAsync(new IdentityRole("Client"));

                string defaultUserName = "admin";
                var defaultUser = new AppUser { UserName = defaultUserName, Email = "mostafabazghandi2001@gmail.com" };
                await userManager.CreateAsync(defaultUser, "Password*");
                defaultUser = await userManager.FindByNameAsync(defaultUserName);
                await userManager.AddToRoleAsync(defaultUser, "Admin");
                await userManager.AddToRoleAsync(defaultUser, "SurveyDesigner");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Can not seed database. {ex.Message}");
            }

        }
    }
}
