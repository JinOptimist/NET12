using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.DbModel.GuessTheNumber;
using WebMaze.EfStuff.DbModel.SeaBattle;
using WebMaze.EfStuff.Repositories;
using WebMaze.EfStuff.Repositories.GuessTheNumber;
using WebMaze.EfStuff.Repositories.SeaBattle;
using WebMaze.Services.GuessTheNumber;

namespace WebMaze.EfStuff
{
    public static class SeedExtention
    {
        public const string DefaultAdminName = "admin";
        public const string DefaultMazeDifficultName = "Default";
        public const string DefaultNewsTitle = "TestNews";
        public const string DefaultImageDesc = "Default admin image";       
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
                SeedSeaBattleDifficult(scope);
                SeedStuffForHero(scope);
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
            var randomNames = new List<string> { "Silaterr", "Yestio", "Whindali", "Shlongi", "Beroldek" };
            var random = new Random();

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

            if (userRepository.Count() < 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    string randomName;

                    do
                    {
                        randomName = randomNames[random.Next(randomNames.Count())] + random.Next(100);
                    }
                    while (userRepository.GetUserByName(randomName) != null);

                    var randomUser = new User()
                    {
                        Name = randomName,
                        Password = "123",
                        Coins = random.Next(10000),
                        Age = random.Next(18, 60),
                        IsActive = true,
                        GlobalUserRating = random.Next(-1000, 1000)
                    };
                    userRepository.Save(randomUser);
                }
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
                    Width = 20,
                    Height = 20,
                    HeroMoney = 10,
                    HeroMaxHp = 50,
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
                    Picture = "/images/galleryImg/defaultGalleryFoto1.png",
                    Assessment = 9,
                    Author = author,
                    IsActive = true
                };
                imageRepository.Save(image);

                image = new Image()
                {
                    Description = "\"Play 15\" ",
                    Picture = "/images/galleryImg/defaultGalleryFoto2.jpg",
                    Assessment = 9,
                    Author = author,
                    IsActive = true
                };
                imageRepository.Save(image);

                image = new Image()
                {
                    Description = "Miner!",
                    Picture = "/images/galleryImg/defaultGalleryFoto3.png",
                    Assessment = 9,
                    Author = author,
                    IsActive = true
                };
                imageRepository.Save(image);

                image = new Image()
                {
                    Description = "Three In Row",
                    Picture = "/images/galleryImg/defaultGalleryFoto4.jpg",
                    Assessment = 9,
                    Author = author,
                    IsActive = true
                };
                imageRepository.Save(image);

                image = new Image()
                {
                    Description = "Zuma",
                    Picture = "/images/galleryImg/defaultGalleryFoto5.jpg",
                    Assessment = 9,
                    Author = author,
                    IsActive = true
                };
                imageRepository.Save(image);

                image = new Image()
                {
                    Description = "Couple",
                    Picture = "/images/galleryImg/defaultGalleryFoto6.jpg",
                    Assessment = 9,
                    Author = author,
                    IsActive = true
                };
                imageRepository.Save(image);

                image = new Image()
                {
                    Description = "Sea battle",
                    Picture = "/images/galleryImg/defaultGalleryFoto7.jpg",
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

                var random = new Random();

                for (int i = 0; i < countTestEntry; i++)
                {
                    var testNewCellSugg = new NewCellSuggestion()
                    {
                        Title = $"TestCellSugg-{i}",
                        IsActive = true,
                        Url = "/imgYellowTeam/stoc.jpg",
                        Creater = userRepository.GetUserByName(namesTestUsers[random.Next(namesTestUsers.Count)])
                    };

                    newCellSuggRepository.Save(testNewCellSugg);
                }

            }

        }

        private static void SeedSeaBattleDifficult(IServiceScope scope)
        {

            var seaBattleDifficult = scope.ServiceProvider.GetService<SeaBattleDifficultRepository>();

            if (!seaBattleDifficult.GetAll().Any())
            {
                var defaultDifficult = new SeaBattleDifficult()
                {
                    Height = 12,
                    Width = 12,
                    FourSizeShip = 2,
                    ThreeSizeShip = 3,
                    TwoSizeShip = 4,
                    IsActive = true
                };
                seaBattleDifficult.Save(defaultDifficult);

                defaultDifficult = new SeaBattleDifficult()
                {
                    Height = 12,
                    Width = 12,
                    FourSizeShip = 3,
                    ThreeSizeShip = 4,
                    TwoSizeShip = 5,
                    IsActive = true
                };
                seaBattleDifficult.Save(defaultDifficult);

                defaultDifficult = new SeaBattleDifficult()
                {
                    Height = 14,
                    Width = 14,
                    FourSizeShip = 4,
                    ThreeSizeShip = 5,
                    TwoSizeShip = 6,
                    IsActive = true
                };
                seaBattleDifficult.Save(defaultDifficult);

                defaultDifficult = new SeaBattleDifficult()
                {
                    Height = 10,
                    Width = 10,
                    FourSizeShip = 1,
                    ThreeSizeShip = 3,
                    TwoSizeShip = 4,
                    IsActive = true
                };
                seaBattleDifficult.Save(defaultDifficult);

                defaultDifficult = new SeaBattleDifficult()
                {
                    Height = 10,
                    Width = 10,
                    FourSizeShip = 1,
                    ThreeSizeShip = 2,
                    TwoSizeShip = 2,
                    IsActive = true
                };
                seaBattleDifficult.Save(defaultDifficult);

                defaultDifficult = new SeaBattleDifficult()
                {
                    Height = 14,
                    Width = 14,
                    FourSizeShip = 1,
                    ThreeSizeShip = 6,
                    TwoSizeShip = 8,
                    IsActive = true
                };
                seaBattleDifficult.Save(defaultDifficult);
            }
        }

        private static void SeedStuffForHero(IServiceScope scope)
        {
            var userRepository = scope.ServiceProvider.GetService<UserRepository>();

            var stuffForHeroRepository = scope.ServiceProvider.GetService<StuffRepository>();

            if (stuffForHeroRepository.Count() < 100)
            {
                var randomNames = new List<string> { "Bow", "Dager", "Sword", "Mace", "Axe" };
                var random = new Random();

                for (int i = 0; i < 100; i++)
                {
                    var stuffForHero = new StuffForHero()
                    {
                        Name = randomNames[random.Next(randomNames.Count())]+ random.Next(1000),
                        Description = "no need to filter, so standard for everyone",
                        Price = random.Next(1000),
                        PictureLink = "https://rozetked.me/images/uploads/dwoilp3BVjlE.jpg",
                        Proposer = userRepository.GetFullRandomUser(),
                        IsActive = true
                    };

                    stuffForHeroRepository.Save(stuffForHero);
                }
            }
        }

    }
}
