using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;

namespace WebMaze.Controllers
{
    public class NewsController : Controller
    {
        private UserRepository _userRepository;
        private NewsRepository _newsRepository;
        private IMapper _mapper;

        public NewsController(UserRepository userRepository, 
            NewsRepository newsRepository, 
            IMapper mapper)
        {
            _newsRepository = newsRepository;
            _userRepository = userRepository;
            _mapper = mapper;
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

        [HttpGet]
        public IActionResult AddNews()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddNews(NewsViewModel newsViewModel)
        {
            var author = _userRepository.GetRandomUser();

            var dbNews = _mapper.Map<News>(newsViewModel);

            dbNews.CreationDate = DateTime.Now.Date;
            dbNews.Author = author;
            dbNews.IsActive = true;

            _newsRepository.Save(dbNews);

            return RedirectToAction("Index", "News");
        }

        public IActionResult RemoveNews(long newsId)
        {
            _newsRepository.Remove(newsId);
            return RedirectToAction("Index", "News");
        }
    }
}
