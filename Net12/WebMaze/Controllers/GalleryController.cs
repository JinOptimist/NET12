using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;

namespace WebMaze.Controllers
{
    public class GalleryController : Controller
    {        
        private ImageRepository _repository;
        private UserRepository _userRepository;

        public GalleryController(ImageRepository repository, UserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var imageViewModels = new List<ImageViewModel>();
            foreach (var dbImage in _repository.GetAll())
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
            ViewBag.Users = new SelectList(_userRepository.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult AddImage(ImageViewModel imageViewModel)
        {
            var dbImage = new Image()
            {
                Author = _userRepository.Get(imageViewModel.Author.Id),
                Description = imageViewModel.Description,
                Picture = imageViewModel.Picture,
                Assessment = imageViewModel.Assessment,
                IsActive = true
            };
            
            _repository.Save(dbImage);            

            return RedirectToAction("Index", "Gallery");            
        }
    }
}
