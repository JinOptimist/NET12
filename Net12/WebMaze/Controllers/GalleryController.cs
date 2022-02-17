using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMaze.Controllers.AuthAttribute;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Models.Enums;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    [Authorize]
    public class GalleryController : Controller
    {
        private readonly ImageRepository _repository;
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly PayForActionService _payForActionService;
        private UserService _userService;
        private IWebHostEnvironment _hostEnvironment;


        public GalleryController(ImageRepository repository,
            UserRepository userRepository,
            IMapper mapper,
            PayForActionService payForActionService,
            UserService userService,
            IWebHostEnvironment hostEnvironment)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
            _payForActionService = payForActionService;
            _userService = userService;
            _hostEnvironment = hostEnvironment;
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
        public IActionResult GalleryCarousel()
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

        [PayForAddActionFilter(TypesOfPayment.Huge)]
        [HttpPost]
        public IActionResult AddImage(ImageViewModel imageViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(imageViewModel);
            }

            var dbImage = _mapper.Map<Image>(imageViewModel);
            dbImage.Author = _userService.GetCurrentUser();
            _repository.Save(dbImage);

            var fileName = $"{dbImage.Id}.jpg";
            dbImage.Picture = "/images/gallery/" + fileName;
            _repository.Save(dbImage);

            var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", "gallery", fileName);
            using (var fileStream = System.IO.File.Create(filePath))
            {
                imageViewModel.ImageFile.CopyTo(fileStream);
            }

            return RedirectToAction("Index", "Gallery");
        }

        public IActionResult Wonderful(long imageId)
        {
            var reward = 10;
            var image = _repository.Get(imageId);
            _payForActionService.CreatorEarnMoney(image.Author.Id, reward);

            return RedirectToAction("Index", "Gallery");
        }

        public IActionResult Awful(long imageId)
        {
            
            var image = _repository.Get(imageId);

            _payForActionService.CreatorDislikeFine(image.Author.Id, TypesOfPayment.Fine);

            return RedirectToAction("Index", "Gallery");
        }
    }
}
