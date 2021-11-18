using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.Models;

namespace WebMaze.Controllers
{
    public class NewsController : Controller
    {
        private WebContext _webContext;

        public NewsController(WebContext webContext)
        {
            _webContext = webContext;
        }

        public IActionResult Index()
        {
            var newsViewModels = new List<NewsViewModel>();
            newsViewModels = _webContext.News.Select(
                x => new NewsViewModel
                {
                    CreationDate = x.CreationDate,
                    EventDate = x.EventDate,
                    Location = x.Location,
                    NameOfAuthor = x.NameOfAuthor,
                    Text = x.Text,
                    Title = x.Title
                }).ToList();

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
            var dbNews = new News()
            {
                EventDate = newsViewModel.EventDate,
                CreationDate = DateTime.Now.Date,
                Location = newsViewModel.Location,
                NameOfAuthor = newsViewModel.NameOfAuthor,
                Text = newsViewModel.Text,
                Title = newsViewModel.Title
            };
            _webContext.News.Add(dbNews);

            _webContext.SaveChanges();

            return RedirectToAction("Index", "News");
        }
    }
}
