using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
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
        private NewCellSuggRepository _newCellSuggRepository;
        private ILogger<HomeController> _logger;
        private CurrenceService _currenceService;
        private BookRepository _bookRepository;

        private IMapper _mapper;
        public HomeController(WebContext webContext,
         UserRepository userRepository, ReviewRepository reviewRepository,
         IMapper mapper, UserService userService,
         NewCellSuggRepository newCellSuggRepository,
         ILogger<HomeController> logger,
         CurrenceService currenceService,
         BookRepository bookRepository)
        {
            _webContext = webContext;
            _userRepository = userRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
            _currenceService = currenceService;
            _bookRepository = bookRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userService.GetCurrentUser()?.Id;
            _logger.Log(LogLevel.Information, $"User {userId} on maion page");

            var userViewModels = _userRepository.GetAll()
                 .Select(x => _mapper.Map<UserViewModel>(x)).ToList();

            var rates = await _currenceService.GetRates();
            var viewModel = new HomeIndexViewModel()
            {
                Users = userViewModels,
                Rates = rates
            };

            return View(viewModel);
        }

        public IActionResult Book()
        {
            var bookViewModels = _bookRepository
                .GetAllSortedByAuthor()
                .Select(Book => _mapper.Map<BookViewModel>(Book))
                .ToList();

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
            if (!ModelState.IsValid)
            {
                return View(bookViewModel);
            }

            var dbBook = _mapper.Map<Book>(bookViewModel);
            dbBook.Creator = _userService.GetCurrentUser();
            _bookRepository.Save(dbBook);            

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

        public IActionResult BoringError()
        {
            return View();
        }

        public IActionResult SecreteError()
        {
            return View();
        }



    }

}
