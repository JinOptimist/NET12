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
        private WebContext _webContext;

        private UserRepository _userRepository;
        private NewsRepository _newsRepository;

        public NewsController(WebContext webContext, UserRepository userRepository,NewsRepository newsRepository)
        {
            _webContext = webContext;
            _newsRepository = newsRepository;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            var newsViewModels = new List<NewsViewModel>();
            newsViewModels = _newsRepository.GetAll()
                .Select(x => new NewsViewModel
                {
                    CreationDate = x.CreationDate,
                    EventDate = x.EventDate,
                    Location = x.Location,
                    NameOfAuthor = x.Author.Name,
                    Text = x.Text,
                    Title = x.Title,
                    Id=x.Id
                }).ToList();

            return View(newsViewModels);
        }

        [HttpGet]
        public IActionResult AddNews()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddNews(News news)
        {
            news.Author = _userRepository.GetRandomUser();
            news.IsActive = true;
            news.CreationDate = DateTime.Now;
            _newsRepository.Save(news);
            return RedirectToAction("Index", "News");
        }

        public IActionResult RemoveNews(long newsId)
        {
            _newsRepository.Remove(newsId);
            return RedirectToAction("Index", "News");
        }
    }
}
