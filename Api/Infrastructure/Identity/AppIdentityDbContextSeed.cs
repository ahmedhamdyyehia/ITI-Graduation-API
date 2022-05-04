using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager , RoleManager<AppRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        DisplayName = "Hamada",
                        Email = "Hamada@gmail.com",
                        UserName = "Hamada@gmail.com",
                        Address = new Address()
                        {
                            FristName = "Hamada",
                            LastName = "Mohamde",
                            Street = "Ahmed Maher",
                            City = "Mansoura",
                            State = "Dakahlia",
                            ZipCode = "45451"
                        }
                    },
                      new AppUser
                    {
                        DisplayName = "Hamada(Admin)",
                        Email = "HamadaAdmin@gmail.com",
                        UserName = "HamadaAdmin@gmail.com",
                    }
                };

                var roles = new List<AppRole>
                {
                    new AppRole {Name="Admin"},
                    new AppRole {Name="Customer"}
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pass@123");
                    await userManager.AddToRoleAsync(user, "Customer");

                    if (user.Email == "HamadaAdmin@gmail.com")
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }
        }
    }
}
