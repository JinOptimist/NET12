using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebContext _webContext;
        private UserService _userService;
        private UserRepository _userRepository;
        private ReviewRepository _reviewRepository;
        private FavGamesRepository _favGamesRepository;
        private NewCellSuggRepository _newCellSuggRepository;
        private IMapper _mapper;
        public HomeController(WebContext webContext,
         UserRepository userRepository, ReviewRepository reviewRepository,
         IMapper mapper, FavGamesRepository favGamesRepository, UserService userService, NewCellSuggRepository newCellSuggRepository)
        {
            _webContext = webContext;
            _userRepository = userRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _userService = userService;
            _favGamesRepository = favGamesRepository;
        }

        public IActionResult Index()
        {
            var userViewModels = _userRepository.GetAll()
                 .Select(x => _mapper.Map<UserViewModel>(x)).ToList();

            return View(userViewModels);
        }

        public IActionResult Book()
        {
            var bookViewModels = new List<BookViewModel>();
            foreach (var dbBook in _webContext.Books)
            {
                var bookViewModel = new BookViewModel();
                bookViewModel.Name = dbBook.Name;
                bookViewModel.Link = dbBook.Link;
                bookViewModel.ImageLink = dbBook.ImageLink;
                bookViewModel.Author = dbBook.Author;
                bookViewModel.Desc = dbBook.Desc;
                bookViewModel.ReleaseDate = dbBook.ReleaseDate;
                bookViewModel.PublicationDate = dbBook.PublicationDate;
                bookViewModels.Add(bookViewModel);
            }

            return View(bookViewModels);
        }

        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddBook(BookViewModel bookViewModel)
        {
            var dbBook = new Book()
            {
                Name = bookViewModel.Name,
                Link = bookViewModel.Link,
                ImageLink = bookViewModel.ImageLink,
                Author = bookViewModel.Author,
                Desc = bookViewModel.Desc,
                ReleaseDate = bookViewModel.ReleaseDate,
                PublicationDate = bookViewModel.PublicationDate
            };
            _webContext.Books.Add(dbBook);

            _webContext.SaveChanges();

            return RedirectToAction("Index", "Home");
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
                Password = userViewMode.Password,
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
        public IActionResult FavoriteGames()
        {
            //var GamesViewModels = new List<GameViewModel>();
            var GamesViewModels = _favGamesRepository
               .GetAll()
               .Select(dbModel => _mapper.Map<GameViewModel>(dbModel))
               .ToList();

            return View(GamesViewModels);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddGame()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddGame(GameViewModel gameViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(gameViewModel);
            }

            var creater = _userService.GetCurrentUser();

            var dbGame = _mapper.Map<Game>(gameViewModel);
            dbGame.Creater = creater;
            dbGame.IsActive = true;

            _favGamesRepository.Save(dbGame);

            return RedirectToAction("FavoriteGames", "Home");
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
