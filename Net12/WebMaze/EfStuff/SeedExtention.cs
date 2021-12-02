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
        public const string DefaultNewsTitle = "TestNews";
        public static IHost Seed(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                SeedUser(scope);
                SeedMazeDifficult(scope);
                SeedNews(scope);
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
        private static void SeedNews(IServiceScope scope)
        {
            var author = scope.ServiceProvider.GetService<UserRepository>().GetUserByName(DefaultAdminName);
            var newsRepository = scope.ServiceProvider.GetService<NewsRepository>();
            var testNews = newsRepository.GetNewsByName(DefaultNewsTitle);
            if (testNews == null)
            {
                testNews = new News()
                {
                    Title = DefaultNewsTitle,
                    IsActive = true,
                    Location = "Maze",
                    Text = "Attention! New cell is soon!",
                    Author=author,
                    CreationDate=DateTime.Now,
                    EventDate=DateTime.Today.AddDays(7)
                };
                newsRepository.Save(testNews);
        }

    }
    private static void SeedMazeDifficult(IServiceScope scope)
    {
        var userRepository = scope.ServiceProvider.GetService<UserRepository>();

        var mazeDifficult = scope.ServiceProvider.GetService<MazeDifficultRepository>();
        var defaultDifficult = mazeDifficult.GetMazeDifficultByName(DefaultMazeDifficultName);

        if (defaultDifficult == null)
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
