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
        private MovieRepository _movieRepository;
        private IMapper _mapper;

        public HomeController(WebContext webContext,
         UserRepository userRepository, ReviewRepository reviewRepository,
         IMapper mapper, UserService userService, MovieRepository movieRepository)
        {
            _webContext = webContext;
            _userRepository = userRepository;
            _reviewRepository = reviewRepository;
            _movieRepository = movieRepository;
            _mapper = mapper;
            _userService = userService;
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






        public IActionResult Movie()
        {
            var MovieViewModels = new List<MovieViewModel>();
            MovieViewModels = _movieRepository
                .GetAll()
                .Select(dbModel => _mapper.Map<MovieViewModel>(dbModel))
                .ToList();


            return View(MovieViewModels);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddMovie(long id)
        {
            var model = _mapper.Map<MovieViewModel>(_movieRepository.Get(id)) 
                ?? new MovieViewModel() {
                    Release = DateTime.Now.Year
                };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddMovie(MovieViewModel movieViewModel)
        {

            var dbMovie = new Movie();
            dbMovie = _mapper.Map<Movie>(movieViewModel);
            dbMovie.IsActive = true;
            _movieRepository.Save(dbMovie);
            return RedirectToAction($"{nameof(HomeController.Movie)}");
        }

        public IActionResult RemoveMovie(long id)
        {
            _movieRepository.Remove(id);
            return RedirectToAction($"{nameof(HomeController.Movie)}");
        }


    }


}


