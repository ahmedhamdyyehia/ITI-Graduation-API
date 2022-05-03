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
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
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
                };

                await userManager.CreateAsync(user, "Pass@123");
            }
        }
    }
}
