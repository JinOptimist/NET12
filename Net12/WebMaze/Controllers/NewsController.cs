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
            foreach (var dbNew in _webContext.News)
            {
                var newsViewModel = new NewsViewModel();
                newsViewModel.DateOfNew = dbNew.DateOfNew;
                newsViewModel.DatNow = dbNew.DatNow;
                newsViewModel.Location = dbNew.Location;
                newsViewModel.NameOfAthor = dbNew.NameOfAthor;
                newsViewModel.Text = dbNew.Text;
                newsViewModel.Title = dbNew.Title;
                newsViewModels.Add(newsViewModel);
            }

            return View(newsViewModels);
        }

        [HttpGet]
        public IActionResult AddNew()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddNew(NewsViewModel newsViewModel)
        {
            var dbNew = new New()
            {
                DateOfNew = newsViewModel.DateOfNew,
                DatNow = DateTime.Now.Date,
                Location = newsViewModel.Location,
                NameOfAthor = newsViewModel.NameOfAthor,
                Text = newsViewModel.Text,
                Title = newsViewModel.Title
            };
            _webContext.News.Add(dbNew);

            _webContext.SaveChanges();

            return RedirectToAction("Index", "News");
        }
    }
}
