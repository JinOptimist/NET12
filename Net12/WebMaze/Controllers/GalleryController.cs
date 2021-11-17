using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.Models;

namespace WebMaze.Controllers
{
    public class GalleryController : Controller
    {
        private WebContext _webContext;

        public GalleryController(WebContext webContext)
        {
            _webContext = webContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var imageViewModels = new List<ImageViewModel>();
            foreach (var dbImage in _webContext.Gallery)
            {
                var imageViewModel = new ImageViewModel();
                imageViewModel.Author = dbImage.Author;
                imageViewModel.Description = dbImage.Description;
                imageViewModel.Assessment = dbImage.Assessment;
                imageViewModel.Picture = dbImage.Picture;

                imageViewModels.Add(imageViewModel);
            }
            return View(imageViewModels);
        }

        [HttpGet]
        public IActionResult AddImage()
        {
            ViewBag.Users = new SelectList(_webContext.Users, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult AddImage(ImageViewModel imageViewModel)
        {
            var dbImage = new Image()
            {
                //Author = imageViewModel.Au,
                Description = imageViewModel.Description,
                Picture = imageViewModel.Picture,
                Assessment = imageViewModel.Assessment
            };
            _webContext.Gallery.Add(dbImage);

            _webContext.SaveChanges();

            return RedirectToAction("Index", "Gallery");            
        }
    }
}
