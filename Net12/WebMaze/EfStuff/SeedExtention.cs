﻿using Microsoft.Extensions.DependencyInjection;
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
                    Author = author
                };
                imageRepository.Save(image);
            }
        }
    }
}
