using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Controllers.AuthAttribute;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class NewsController : Controller
    {
        private UserRepository _userRepository;
        private NewsRepository _newsRepository;
        private IMapper _mapper;
        private UserService _userService;
        private readonly PayForActionService _payForActionService;

        public NewsController(UserRepository userRepository,
            NewsRepository newsRepository,
            IMapper mapper, UserService userService,
             PayForActionService payForActionService)
        {
            _newsRepository = newsRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;
            _payForActionService = payForActionService;
        }

        public IActionResult Index()
        {
            var newsViewModels = new List<NewsViewModel>();
            newsViewModels = _newsRepository
                .GetAll()
                .Select(dbModel => _mapper.Map<NewsViewModel>(dbModel))
                .ToList();

            return View(newsViewModels);
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
        [HttpPost]
        public IActionResult AddNews(NewsViewModel newsViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(newsViewModel);
            }

            if (!_payForActionService.Payment(200))
            {
                ModelState.AddModelError(string.Empty, "Not enought money to add news");
                return View(newsViewModel);
            }

            var author = _userService.GetCurrentUser();

            var dbNews = _mapper.Map<News>(newsViewModel);

            dbNews.CreationDate = DateTime.Now.Date;
            dbNews.Author = author;
            dbNews.IsActive = true;

            _newsRepository.Save(dbNews);

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
    }
}
