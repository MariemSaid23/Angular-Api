using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities.Identity;

namespace Talabat.Repositery.Identity
{
    public static class ApplicationIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {

                var user = new ApplicationUser()
                {
                    DisplayName = "Maryam Said",
                    Email = "maryamelfishaw7@gmail.com",
                    UserName = "MaryamElfishawy",
                    PhoneNumber = "01125272659"
                };
                await userManager.CreateAsync(user, "P@ssw0rd");

            }


        }
    }
}
