﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.DbModel.GuessTheNumber;
using WebMaze.EfStuff.Repositories;
using WebMaze.EfStuff.Repositories.GuessTheNumber;
using WebMaze.Services.GuessTheNumber;

namespace WebMaze.EfStuff
{
    public static class SeedExtention
    {
        public const string DefaultAdminName = "admin";
        public const string DefaultMazeDifficultName = "Default";
        public const string DefaultNewsTitle = "TestNews";
        public const string DefaultImageDesc = "Admin image";
        public static IHost Seed(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                SeedUser(scope);
                SeedMazeDifficult(scope);
                SeedNews(scope);
                SeedPermissions(scope);
                SeedGallery(scope);
                SeedZumaGameDifficult(scope);
                SeedGuessTheNumberGameParametersRecords(scope);
                SeedNewCellSugg(scope);
            }

            return host;
        }

        private static void SeedPermissions(IServiceScope scope)
        {
            var permissionRepository = scope.ServiceProvider.GetService<PermissionRepository>();
            var perrmissions = permissionRepository.GetAll();
            if (!perrmissions.Any())
            {
                var adminPermission = new Perrmission()
                {
                    Name = Perrmission.Admin,
                    Desc = "admin",
                    UsersWhichHasThePermission = new List<User>(),
                    IsActive = true
                };
                var admin = scope.ServiceProvider.GetService<UserRepository>().GetUserByName(DefaultAdminName);
                adminPermission.UsersWhichHasThePermission.Add(admin);
                permissionRepository.Save(adminPermission);

                permissionRepository.Save(new Perrmission()
                {
                    Name = Perrmission.NewsCreator,
                    Desc = "News creator",
                    IsActive = true
                });

                permissionRepository.Save(new Perrmission()
                {
                    Name = Perrmission.ForumModerator,
                    Desc = "Forum Moderator",
                    IsActive = true
                });
            }
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
                    Age = 32,
                    IsActive = true,
                    GlobalUserRating = 9999
                };

                userRepository.Save(admin);
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
                    Author = author,
                    CreationDate = DateTime.Now,
                    EventDate = DateTime.Today.AddDays(7)
                };
                newsRepository.Save(testNews);
            }

            if (newsRepository.Count() < 200)
            {
                for (int i = 0; i < 200; i++)
                {
                    var news = new News()
                    {
                        Title = DefaultNewsTitle + i,
                        IsActive = true,
                        Location = $"Maze {i}",
                        Text = $"Attention! {i}",
                        Author = author,
                        CreationDate = DateTime.Now.AddDays(-1 * i),
                        EventDate = DateTime.Today.AddDays(-1 * i)
                    };
                    newsRepository.Save(news);
                }
            }
        }

        private static void SeedGallery(IServiceScope scope)
        {
            var author = scope.ServiceProvider.GetService<UserRepository>().GetUserByName(DefaultAdminName);
            var imageRepository = scope.ServiceProvider.GetService<ImageRepository>();
            var image = imageRepository.GetImageByDesc(DefaultImageDesc);
            if (image == null)
            {
                image = new Image()
                {
                    Description = DefaultImageDesc,
                    Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSvgRMad98wVTdc-qAMIhYEF6tJ0QVKdJ03oA&usqp=CAU",
                    Assessment = 9,
                    Author = author,
                    IsActive = true
                };
                imageRepository.Save(image);
            }
        }

        private static void SeedZumaGameDifficult(IServiceScope scope)
        {
            var zumaGameDifficult = scope.ServiceProvider.GetService<ZumaGameDifficultRepository>();
            var defaultDifficults = zumaGameDifficult.GetAll();

            var userAdmin = scope.ServiceProvider.GetService<UserRepository>().GetUserByName(DefaultAdminName);

            if (!defaultDifficults.Any())
            {
                var defaultDifficult = new ZumaGameDifficult()
                {
                    Width = 10,
                    Height = 10,
                    ColorCount = 3,
                    Price = 100,
                    IsActive = true,
                    Author = userAdmin
                };

                zumaGameDifficult.Save(defaultDifficult);
            }

        }
        private static void SeedGuessTheNumberGameParametersRecords(IServiceScope scope)
        {
            var seedGameParametrs = scope.ServiceProvider.GetService<GuessTheNumberGameParametersRepository>();
            var guessTheNumberGameParameters = seedGameParametrs.GetAll();

            if (!guessTheNumberGameParameters.Any())
            {

                var guessTheNumberGameParameterEasy = new GuessTheNumberGameParameters()
                {
                    Difficulty = GuessTheNumberGameDifficulty.Easy,
                    MinRangeNumber = GuessTheNumberGameConstans.MinRangeNumberEasy,
                    MaxRangeNumber = GuessTheNumberGameConstans.MaxRangeNumberEasy,
                    GameCost = GuessTheNumberGameConstans.GameCostEasy,
                    RewardForWinningTheGame = GuessTheNumberGameConstans.RewardForWinningTheGameEasy,
                    MaxAttempts = 4,
                    IsActive = true
                };
                seedGameParametrs.Save(guessTheNumberGameParameterEasy);

                var guessTheNumberGameParameterMedium = new GuessTheNumberGameParameters()
                {
                    Difficulty = GuessTheNumberGameDifficulty.Medium,
                    MinRangeNumber = GuessTheNumberGameConstans.MinRangeNumberMedium,
                    MaxRangeNumber = GuessTheNumberGameConstans.MaxRangeNumberMedium,
                    GameCost = GuessTheNumberGameConstans.GameCostMedium,
                    RewardForWinningTheGame = GuessTheNumberGameConstans.RewardForWinningTheGameMedium,
                    MaxAttempts = 7,
                    IsActive = true
                };
                seedGameParametrs.Save(guessTheNumberGameParameterMedium);

                var guessTheNumberGameParameterHard = new GuessTheNumberGameParameters()
                {
                    Difficulty = GuessTheNumberGameDifficulty.Hard,
                    MinRangeNumber = GuessTheNumberGameConstans.MinRangeNumberHard,
                    MaxRangeNumber = GuessTheNumberGameConstans.MaxRangeNumberHard,
                    GameCost = GuessTheNumberGameConstans.GameCostHard,
                    RewardForWinningTheGame = GuessTheNumberGameConstans.RewardForWinningTheGameHard,
                    MaxAttempts = 10,
                    IsActive = true
                };
                seedGameParametrs.Save(guessTheNumberGameParameterHard);
            }
        }

        private static void SeedNewCellSugg(IServiceScope scope)
        {
            var newCellSuggRepository = scope.ServiceProvider.GetService<NewCellSuggRepository>();
            var userRepository = scope.ServiceProvider.GetService<UserRepository>();
            int countTestEntry = 40; // Here you can set the required number of test records in the database NewCellSuggestions.

            if (newCellSuggRepository.Count() < countTestEntry)
            {
                var namesTestUsers = new List<string>() { "Bob", "Sam", "Tom", "Mike" }; //To add another test user just add a new name. 

                for (int i = 0; i < namesTestUsers.Count; i++)
                {
                    if (userRepository.GetUserByName(namesTestUsers[i]) == null)
                    {
                        var testUser = new User()
                        {
                            Name = namesTestUsers[i],
                            Password = "1234",
                            Coins = 100,
                            Age = 18,
                            IsActive = true,
                            GlobalUserRating = 100
                        };

                        userRepository.Save(testUser);
                    }

                }

                for (int i = 0; i < countTestEntry; i++)
                {
                    var testNewCellSugg = new NewCellSuggestion()
                    {
                        Title = $"TestCellSugg-{i}",
                        IsActive = true,
                        Url = "/imgYellowTeam/stoc.jpg",
                        Creater = userRepository.GetUserByName(namesTestUsers[new Random().Next(namesTestUsers.Count)])
                    };

                    newCellSuggRepository.Save(testNewCellSugg);
                }

            }

        }
    }
}
