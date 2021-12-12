using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMaze.Controllers.AuthAttribute;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    [Authorize]
    [IsAdmin]
    public class GalleryController : Controller
    {        
        private readonly ImageRepository _repository;
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly PayForActionService _payForActionService;
        private UserService _userService;

        public GalleryController(ImageRepository repository, 
            UserRepository userRepository, 
            IMapper mapper, 
            PayForActionService payForActionService,
            UserService userService)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
            _payForActionService = payForActionService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var imageViewModels = new List<ImageViewModel>();

            if (_userRepository.GetAll().Any())
            {
                imageViewModels = _repository.GetAll().Select(image => _mapper.Map<ImageViewModel>(image)).ToList();
            }

            return View(imageViewModels);
        }

        [HttpGet]
        public IActionResult AddImage()
        {           
            return View();
        }

        [HttpPost]
        public IActionResult AddImage(ImageViewModel imageViewModel)
        {

            if (!_payForActionService.Payment(200))
            {
                ModelState.AddModelError(string.Empty, "Not enought money to add image");
                return View(imageViewModel);
            }

            var dbImage = _mapper.Map<Image>(imageViewModel);
            dbImage.Author = _userService.GetCurrentUser();

            _repository.Save(dbImage);            

            return RedirectToAction("Index", "Gallery");            
        }

        public IActionResult Wonderful(long imageId)
        {
            var image = _repository.Get(imageId);
            _payForActionService.EarnMoney(image.Author.Id, 10);

            return RedirectToAction("Index", "Gallery");
        }
    }
}
