﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly ImageRepository _repository;
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;

        public GalleryController(ImageRepository repository, UserRepository userRepository, IMapper mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
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
            ViewBag.Users = new SelectList(_userRepository.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult AddImage(ImageViewModel imageViewModel)
        {
            var dbImage = _mapper.Map<Image>(imageViewModel);
            dbImage.Author = _userRepository.Get(imageViewModel.Author.Id);
                       
            _repository.Save(dbImage);            

            return RedirectToAction("Index", "Gallery");            
        }
    }
}
