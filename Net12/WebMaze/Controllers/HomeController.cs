using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;

namespace WebMaze.Controllers
{
    public class HomeController : Controller
    {
        private WebContext _webContext;

        private UserRepository _userRepository;
        private ReviewRepository _reviewRepository;

        public HomeController(WebContext webContext, 
            UserRepository userRepository, ReviewRepository reviewRepository)
        {
            _webContext = webContext;
            _userRepository = userRepository;
            _reviewRepository = reviewRepository;
        }

        public IActionResult Index()
        {
            var userViewModels = new List<UserViewModel>();
            foreach (var dbUser in _userRepository.GetAll())
            {
                var userViewModel = new UserViewModel();
                userViewModel.Id = dbUser.Id;
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
                gameViewModel.Age = dbGame.Creater.Age;
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
                .Where(x => x.Name == gameViewMode.Username)
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
                Age = DateTime.Now.Second % 10 + 20,
                IsActive = true

            };

            _userRepository.Save(dbUser);
            
            return RedirectToAction("Index", "Home");
        }

        public IActionResult RemoveUser(long userId)
        {
            _userRepository.Remove(userId);
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


        [HttpGet]
        public IActionResult Reviews()
        {
            var FeedBackUsers = new List<FeedBackUserViewModel>();
            if (_userRepository.GetAll().Any())
            {
                FeedBackUsers = _reviewRepository.GetAll().Select(rev => new FeedBackUserViewModel { UserName = rev.Creator.Name, TextInfo = rev.Text , Rate = rev.Rate}).ToList();
            }

                return View(FeedBackUsers);
        }

        [HttpPost]
        public IActionResult Reviews(Review review)
        {
            // TODO: Selected User
            review.Creator = _userRepository.GetRandomUser();
            review.IsActive = true;
            _reviewRepository.Save(review);

            var FeedBackUsers = new List<FeedBackUserViewModel>();
            if (_reviewRepository.GetAll().Any())
            {
                FeedBackUsers = _reviewRepository.GetAll().Select(rev => new FeedBackUserViewModel { UserName = rev.Creator.Name, TextInfo = rev.Text, Rate = rev.Rate}).ToList();
            }
            return View(FeedBackUsers);
        }


        public IActionResult NewCellSugg()
        {
            var newCellSuggestionsViewModel = new List<NewCellSuggestionViewModel>();
            var suggestions = _webContext.NewCellSuggestions.ToList();
            foreach (var dbNewCellSuggestions in suggestions)
            {
                var newCellSuggestionViewModel = new NewCellSuggestionViewModel();
                newCellSuggestionViewModel.Title = dbNewCellSuggestions.Title;
                newCellSuggestionViewModel.Description = dbNewCellSuggestions.Description;
                newCellSuggestionViewModel.MoneyChange = dbNewCellSuggestions.MoneyChange;
                newCellSuggestionViewModel.HealtsChange = dbNewCellSuggestions.HealtsChange;
                newCellSuggestionViewModel.FatigueChange = dbNewCellSuggestions.FatigueChange;
                newCellSuggestionViewModel.UserName = dbNewCellSuggestions.Creater.Name;

                newCellSuggestionsViewModel.Add(newCellSuggestionViewModel);
            }
            return View("/Views/Home/NewCellSugg.cshtml", newCellSuggestionsViewModel);
        }
        [HttpGet]
        public IActionResult AddNewCellSugg()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNewCellSugg(NewCellSuggestionViewModel newCell)
        {
            //TODO user current user after login
            var creater = _userRepository
                .GetAll()
                .OrderByDescending(x => x.Coins)
                .FirstOrDefault();
            var NewCS = new NewCellSuggestion()
            {
                Title = newCell.Title,
                Description = newCell.Description,
                MoneyChange = newCell.MoneyChange,
                HealtsChange = newCell.HealtsChange,
                FatigueChange = newCell.FatigueChange,
                Creater = creater
            };

            _webContext.NewCellSuggestions.Add(NewCS);
            _webContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


    }

}
