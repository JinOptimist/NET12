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
using System.Threading;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Models.GenerationDocument;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class HomeController : Controller
    {
        public static List<DocumentGenerationTaskInfo> DocumentGenerationTasks
            = new List<DocumentGenerationTaskInfo>();

        private readonly WebContext _webContext;
        private UserService _userService;
        private UserRepository _userRepository;
        private ReviewRepository _reviewRepository;
        private ILogger<HomeController> _logger;
        private CurrenceService _currenceService;

        private IMapper _mapper;
        public HomeController(WebContext webContext,
         UserRepository userRepository, ReviewRepository reviewRepository,
         IMapper mapper, UserService userService,
         ILogger<HomeController> logger,
         CurrenceService currenceService)
        {
            _webContext = webContext;
            _userRepository = userRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
            _currenceService = currenceService;
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
        public IActionResult AddUser(UserViewModel userViewModel)
        {
            var dbUser = new User()
            {
                Name = userViewModel.UserName,
                Password = userViewModel.Password,
                Coins = userViewModel.Coins,
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

        public IActionResult StartTask()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            DocumentGenerationTaskInfo documentGeneration;
            lock (DocumentGenerationTasks)
            {
                documentGeneration = new DocumentGenerationTaskInfo()
                {
                    Id = DocumentGenerationTasks.Any()
                        ? DocumentGenerationTasks.Max(x => x.Id) + 1
                        : 1,
                    Percent = 0,
                    CancellationTokenSource = cancellationTokenSource,
                    Document = "Start\r\n"
                };
                DocumentGenerationTasks.Add(documentGeneration);
            }

            var token = cancellationTokenSource.Token;

            var task = new Task(() => Timer(documentGeneration), token);
            task.Start();

            return Json(documentGeneration.Id);
        }

        public IActionResult Kill(int documentId)
        {
            var doc = DocumentGenerationTasks.First(x => x.Id == documentId);
            doc.CancellationTokenSource.Cancel();
            lock (DocumentGenerationTasks)
            {
                DocumentGenerationTasks.Remove(doc);
            }

            return Json(true);
        }

        public IActionResult GetDocument(int id)
        {
            var doc = DocumentGenerationTasks.First(x => x.Id == id);
            return Json(doc.Document);
        }

        private void Timer(DocumentGenerationTaskInfo documentGeneration)
        {
            for (int i = 0; i < 100; i++)
            {
                documentGeneration.Document += $" {documentGeneration.Percent} ";
                documentGeneration.Percent++;
                documentGeneration
                    .CancellationTokenSource
                    .Token
                    .ThrowIfCancellationRequested();
                Thread.Sleep(1000);
            }
        }
    }
}
