using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class StuffController : Controller
    {
        private UserRepository _userRepository;
        private StuffRepository _staffForHeroRepository;
        private IMapper _mapper;
        private UserService _userService;

        public StuffController(UserRepository userRepository,
            StuffRepository stuffForHeroRepository,
            IMapper mapper, UserService userService)
        {
            _staffForHeroRepository = stuffForHeroRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public IActionResult Stuff()
        {
            var staffsForHero = new List<StuffForHeroViewModel>();
            staffsForHero = _staffForHeroRepository
                    .GetAll().Select(dbModel => _mapper.Map<StuffForHeroViewModel>(dbModel)).ToList();
            return View(staffsForHero);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddStuffForHero()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddStuffForHero(StuffForHeroViewModel stuffForHeroViewModel)
        {
            var proposer = _userService.GetCurrentUser();

            var dbStuffForHero = _mapper.Map<StuffForHero>(stuffForHeroViewModel);

            dbStuffForHero.Proposer = proposer;
            dbStuffForHero.IsActive = true;

            _staffForHeroRepository.Save(dbStuffForHero);
            return RedirectToAction("AddStuffForHero");
        }
    }
}
