using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Controllers.AuthAttribute;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Models.Enums;
using WebMaze.Services;
using WebMaze.SignalRHubs;

namespace WebMaze.Controllers
{
    public class NewsController : Controller
    {
        private UserRepository _userRepository;
        private NewsRepository _newsRepository;
        private IMapper _mapper;
        private UserService _userService;
        private readonly PayForActionService _payForActionService;
        private IHubContext<ChatHub> _chatHub;

        public NewsController(UserRepository userRepository,
            NewsRepository newsRepository,
            IMapper mapper, UserService userService,
            PayForActionService payForActionService,
            IHubContext<ChatHub> chatHub)
        {
            _newsRepository = newsRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;
            _payForActionService = payForActionService;
            _chatHub = chatHub;
        }

        public IActionResult Index(int page = 1, int perPage = 13, string typeSorted = "CreationDate")
        {

            var test = _newsRepository.GetAllSorted();


            var newsViewModels = new List<NewsViewModel>();
            newsViewModels = _newsRepository
                .GetForPagination(perPage, page, typeSorted)
                .Select(dbModel => _mapper.Map<NewsViewModel>(dbModel))
                .ToList();

            var paggerViewModel = new PaggerViewModel<NewsViewModel>();

            paggerViewModel.Records = newsViewModels;
            paggerViewModel.TotalRecordsCount = _newsRepository.Count();
            paggerViewModel.PerPage = perPage;
            paggerViewModel.CurrPage = page;

            return View(paggerViewModel);
        }

        [IsAdmin]
        [HttpGet]
        public IActionResult AddNews(long newsId)
        {
            var model = _mapper.Map<NewsViewModel>(_newsRepository.Get(newsId))
                ?? new NewsViewModel()
                {
                    EventDate = DateTime.Now
                };
            return View(model);
        }

        [IsAdmin]
        [PayForAddActionFilter(TypesOfPayment.Medium)]
        [HttpPost]
        public IActionResult AddNews(NewsViewModel newsViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(newsViewModel);
            }

            var author = _userService.GetCurrentUser();

            var dbNews = _mapper.Map<News>(newsViewModel);

            dbNews.CreationDate = DateTime.Now.Date;
            dbNews.Author = author;
            dbNews.IsActive = true;

            _newsRepository.Save(dbNews);

            _chatHub.Clients.All.SendAsync("Added news", _userService.GetCurrentUser().Name);

            return RedirectToAction("Index", "News");
        }

        public IActionResult Wonderful(long newsId)
        {
            var news = _newsRepository.Get(newsId);
            _payForActionService.CreatorEarnMoney(news.Author.Id, 10);

            return RedirectToAction("Index", "News");
        }

        public IActionResult RemoveNews(long newsId)
        {
            _newsRepository.Remove(newsId);
            return RedirectToAction("Index", "News");
        }

        public IActionResult DownloadRecentNews()
        {
            var news = _newsRepository.GetAll();
            using (var ms = new MemoryStream())
            {
                using (var wordDocument = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document))
                {
                    var mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = mainPart.Document.AppendChild(new Body());

                    foreach (var oneNews in news)
                    {
                        var para = body.AppendChild(new Paragraph());

                        var runTitle = para.AppendChild(new Run());
                        runTitle.AppendChild(new Text(oneNews.Title));

                        var properties = new ParagraphProperties();
                        var fontSize = new FontSize() { Val = "36" };
                        properties.Append(fontSize);

                        para.Append(properties);

                        var paraText = body.AppendChild(new Paragraph());
                        var runNewsBody = paraText.AppendChild(new Run());
                        runNewsBody.AppendChild(new Text(oneNews.Text));
                    }

                    wordDocument.Close();
                }

                return File(ms.ToArray(),
                   "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                   "SpecailForYou.docx");
            }
        }
    }
}
