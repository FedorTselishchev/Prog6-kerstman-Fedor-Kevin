using KerstmanPROG6_Fedor_Kevin.Data;
using KerstmanPROG6_Fedor_Kevin.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;

namespace KerstmanPROG6_Fedor_Kevin.Models
{
    public class Seeder
    {
        public static void Initialize(WebApplication app)
        {

            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            ApplicationDbContext context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            //var context = serviceProvider.GetService<ApplicationDbContext>();

            string[] roles = new string[] { "Santa" };

            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role))
                {
                    roleStore.CreateAsync(new IdentityRole(role));
                }
            }


            var user = new IdentityUser
            {
                Email = "Santa@northpole.com",
                NormalizedEmail = "SANTA@NORTHPOLE.COM",
                UserName = "Santa",
                NormalizedUserName = "SANTA",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };


            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<IdentityUser>();
                var hashed = password.HashPassword(user, "secret");
                user.PasswordHash = hashed;

                var userStore = new UserStore<IdentityUser>(context);
                var result = userStore.CreateAsync(user);

            }

            // _ = AssignRoles(serviceProvider, user.Email, roles);

            context.SaveChangesAsync();
        }

        public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email, string[] roles)
        {
         
            UserManager<IdentityUser> _userManager = services.GetService<UserManager<IdentityUser>>();
            IdentityUser user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.AddToRolesAsync(user, roles);

            return result;
        }
    }
}
