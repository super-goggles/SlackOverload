using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SlackOverload.Data;

namespace SlackOverload.Models
{
    public class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (!context.Roles.Any())
            {
                string newRole = new string("Admin");
                await roleManager.CreateAsync(new IdentityRole(newRole));
            }

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            ApplicationUser firstAdmin = new ApplicationUser
            {
                Email = "admin@slackoverload.ca",
                NormalizedEmail = "ADMIN@SLACKOVERLOAD.CA",
                UserName = "admin@slackoverload.ca",
                EmailConfirmed = true
            };

            var hashedPassword = passwordHasher.HashPassword(firstAdmin, "P@assword1");
            firstAdmin.PasswordHash = hashedPassword;
            await userManager.CreateAsync(firstAdmin);
            await userManager.AddToRoleAsync(firstAdmin, "Admin");

            ApplicationUser user1 = new ApplicationUser { Email = "damola@gmail.com", NormalizedEmail = "DAMOLA@GMAIL.COM", UserName = "damola@gmail.com", EmailConfirmed = true };
            ApplicationUser user2 = new ApplicationUser { Email = "taiwo@gmail.com", NormalizedEmail = "TAIWO@GMAIL.COM", UserName = "taiwo@gmail.com", EmailConfirmed = true };
            ApplicationUser user3 = new ApplicationUser { Email = "kenny@gmail.com", NormalizedEmail = "KENNY@GMAIL.COM", UserName = "kenny@gmail.com", EmailConfirmed = true };

            var hashedPassword2 = passwordHasher.HashPassword(user1, "P@ssword1");
            user1.PasswordHash = hashedPassword2;
            await userManager.CreateAsync(user1);

            var hashedPassword3 = passwordHasher.HashPassword(user2, "P@ssword2");
            user2.PasswordHash = hashedPassword3;
            await userManager.CreateAsync(user2);

            var hashedPassword4 = passwordHasher.HashPassword(user3, "P@ssword3");
            user3.PasswordHash = hashedPassword4;
            await userManager.CreateAsync(user3);

            context.SaveChanges();
        }
    }
}
