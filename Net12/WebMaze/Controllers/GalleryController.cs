using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class GalleryController : Controller
    {        
        private readonly ImageRepository _repository;
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly PayForActionService _payForActionService;

        public GalleryController(ImageRepository repository, UserRepository userRepository, IMapper mapper, PayForActionService payForActionService)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
            _payForActionService = payForActionService;
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

        [Authorize]
        [HttpGet]
        public IActionResult AddImage()
        {
            ViewBag.Users = new SelectList(_userRepository.GetAll(), "Id", "Name");
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddImage(ImageViewModel imageViewModel)
        {
            ViewBag.Users = new SelectList(_userRepository.GetAll(), "Id", "Name");

            if (!_payForActionService.Payment(imageViewModel.Author.Id, 200))
            {
                ModelState.AddModelError(string.Empty, "Not enought money to add image");
                return View(imageViewModel);
            }

            var dbImage = _mapper.Map<Image>(imageViewModel);
            dbImage.Author = _userRepository.Get(imageViewModel.Author.Id);
                       
            _repository.Save(dbImage);            

            return RedirectToAction("Index", "Gallery");            
        }

        public IActionResult Wonderful(long userId)
        {
            _payForActionService.EarnMoney(userId, 10);

            return RedirectToAction("Index", "Gallery");
        }
    }
}
