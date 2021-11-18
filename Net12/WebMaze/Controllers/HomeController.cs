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
        private readonly WebContext _webContext;

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

        public IActionResult Stuff()
        {
            var staffsForHero = new List<StaffForHeroViewModel>();
            foreach (var dbStaff in _webContext.StuffsForHero)
            {
                var staffForHeroViewModel = new StaffForHeroViewModel();
                staffForHeroViewModel.Name = dbStaff.Name;
                staffForHeroViewModel.Description = dbStaff.Description;
                staffForHeroViewModel.PictureLink = dbStaff.PictureLink;
                staffForHeroViewModel.Price = dbStaff.Price;
                staffsForHero.Add(staffForHeroViewModel);
            }
            return View(staffsForHero);
        }

        [HttpGet]
        public IActionResult AddStuffForHero()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStuffForHero(StaffForHeroViewModel staffForHeroViewModel)
        {
            var dbStuffForHero = new StuffForHero()
            {
                Name = staffForHeroViewModel.Name,
                Description = staffForHeroViewModel.Description,
                PictureLink = staffForHeroViewModel.PictureLink,
                Price = staffForHeroViewModel.Price
            };
            _webContext.StuffsForHero.Add(dbStuffForHero);
            _webContext.SaveChanges();
            return View();
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
            var creater = _webContext
                .Users
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
