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
using WebMaze.Models.Enums;
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

        public IActionResult Book(BookFilter? bookFilter, bool asc)
        {
            var bookViewModels = _bookRepository
                .GetAllSortedByParam(bookFilter, asc)
                .Select(Book => _mapper.Map<BookViewModel>(Book))
                .ToList();

            return View(new SortedBooksViewModel() 
            { 
                BookFilter = bookFilter, 
                Asc = asc,
                Books = bookViewModels
            });
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
