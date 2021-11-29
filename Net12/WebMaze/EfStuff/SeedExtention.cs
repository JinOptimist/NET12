using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;

namespace WebMaze.EfStuff
{
    public static class SeedExtention
    {
        public const string DefaultAdminName = "admin";
        public static IHost Seed(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                SeedUser(scope);
            }

            return host;
        }

        private static void SeedUser(IServiceScope scope)
        {
            var userRepository = scope.ServiceProvider.GetService<UserRepository>();
            var admin = userRepository.GetUserByName(DefaultAdminName);
            if (admin == null)
            {
                admin = new User()
                {
                    Name = DefaultAdminName,
                    Password = "admin",
                    Coins = 100,
                    IsActive = true
                };

                userRepository.Save(admin);
            }
        }
    }
}
