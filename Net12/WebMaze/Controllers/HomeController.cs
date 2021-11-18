using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.Models;

namespace WebMaze.Controllers
{
    public class HomeController : Controller
    {
        private WebContext _webContext;

        public HomeController(WebContext webContext)
        {
            _webContext = webContext;
        }

        public IActionResult Index()
        {
            var userViewModels = new List<UserViewModel>();
            foreach (var dbUser in _webContext.Users)
            {
                var userViewModel = new UserViewModel();
                userViewModel.UserName = dbUser.Name;
                userViewModel.Coins = dbUser.Coins;
                userViewModels.Add(userViewModel);
            }

            //var userViewModels2 = _webContext.Users.Select(
            //    dbModel => new UserViewModel { 
            //        UserName = dbModel.Name, 
            //        Coins = dbModel.Coins 
            //    });

            return View(userViewModels);
        }

        public IActionResult FavoriteGames()
        {
            var GamesViewModels = new List<GameViewModel>();
            var games = _webContext.FavGames.ToList();
            foreach (var dbGame in games)
            {
                var gameViewModel = new GameViewModel();
                gameViewModel.Name = dbGame.Name;
                gameViewModel.Genre = dbGame.Genre;
                gameViewModel.YearOfProd = dbGame.YearOfProd;
                gameViewModel.Desc = dbGame.Desc;
                gameViewModel.Rating = dbGame.Rating;
                gameViewModel.Username = dbGame.Creater.Name;
                GamesViewModels.Add(gameViewModel);
            }

            return View(GamesViewModels);
        }

        [HttpGet]
        public IActionResult AddGame()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddGame(GameViewModel gameViewMode)
        {
            var creater = _webContext.Users
                .OrderBy(x => x.Coins)
                .FirstOrDefault();

            var dbGame = new Game()
            {
                Name = gameViewMode.Name,
                Genre = gameViewMode.Genre,
                YearOfProd = gameViewMode.YearOfProd,
                Desc = gameViewMode.Desc,
                Rating = gameViewMode.Rating,
                Creater = creater,
            };
            _webContext.FavGames.Add(dbGame);

            _webContext.SaveChanges();

            return RedirectToAction("FavoriteGames", "Home");
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(UserViewModel userViewMode)
        {
            var dbUser = new User()
            {
                Name = userViewMode.UserName,
                Coins = userViewMode.Coins,
                Age = DateTime.Now.Second % 10 + 20
            };
            _webContext.Users.Add(dbUser);

            _webContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Time()
        {
            var smile = DateTime.Now.Second;
            return View(smile);
        }

        [HttpGet]
        public IActionResult Sum()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Sum(int x, int y)
        {
            var model = x + y;
            return View(model);
        }
    }
}
