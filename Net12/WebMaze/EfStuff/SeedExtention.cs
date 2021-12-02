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
        public const string DefaultMazeDifficultName = "Default";
        public static IHost Seed(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                SeedUser(scope);
                SeedMazeDifficult(scope);
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

        private static void SeedMazeDifficult(IServiceScope scope)
        {
            var userRepository = scope.ServiceProvider.GetService<UserRepository>();

            var mazeDifficult = scope.ServiceProvider.GetService<MazeDifficultRepository>();
            var defaultDifficult = mazeDifficult.GetMazeDifficultByName(DefaultMazeDifficultName);

            if(defaultDifficult == null)
            {
                defaultDifficult = new MazeDifficultProfile()
                {
                    Name = DefaultMazeDifficultName,
                    Width = 10,
                    Height = 10,
                    HeroMoney = 100,
                    HeroMaxHp = 100,
                    HeroMaxFatigue = 30,
                    CoinCount = 5,
                    IsActive = true,
                    Creater = userRepository.GetUserByName(DefaultAdminName)
                };

                mazeDifficult.Save(defaultDifficult);
            }

        }
    }
}
