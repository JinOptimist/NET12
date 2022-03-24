﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebMaze.Controllers.AuthAttribute;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Models.Enums;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class StuffController : Controller
    {
        private UserRepository _userRepository;
        private StuffRepository _staffForHeroRepository;
        private IMapper _mapper;
        private UserService _userService;
        private readonly PayForActionService _payForActionService;

        public StuffController(UserRepository userRepository,
            StuffRepository stuffForHeroRepository,
            IMapper mapper, UserService userService,
            PayForActionService payForActionService)
        {
            _staffForHeroRepository = stuffForHeroRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;
            _payForActionService = payForActionService;
        }

        public IActionResult Stuff(int page = 1, int perPage = 10)
        {

            var stuffsForHeroViewModel = _staffForHeroRepository
                    .GetQueryableForPagination(perPage, page)
                    .Select(dbModel => _mapper.Map<StuffForHeroViewModel>(dbModel))
                    .ToList();

            var filteredColumnNames = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.Name == "StuffForHeroViewModel")
                .FirstOrDefault()
                .GetProperties()
                .Select(x => x.Name)
                .ToList();

            filteredColumnNames.Remove("Id");
            filteredColumnNames.Remove("PictureLink");

            var stuffsForHeroIndexViewModel = new StuffForHeroIndexViewModel
            {
                StuffForHeroViewModels = new PaggerViewModel<StuffForHeroViewModel>
                {
                    PerPage = perPage,
                    CurrPage = page,
                    Records = stuffsForHeroViewModel
                },
                FilteredColumnNames = filteredColumnNames
            };

            return View(stuffsForHeroIndexViewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddStuffForHero(long stuffId)
        {
            var model = _mapper.Map<StuffForHeroViewModel>(_staffForHeroRepository.Get(stuffId))
                ?? new StuffForHeroViewModel(); 
            return View(model);
        }

        [Authorize]
        [PayForAddActionFilter(TypesOfPayment.Huge)]
        [HttpPost]
        public IActionResult AddStuffForHero(StuffForHeroViewModel stuffForHeroViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(stuffForHeroViewModel);
            }

            var proposer = _userService.GetCurrentUser();

            var dbStuffForHero = _mapper.Map<StuffForHero>(stuffForHeroViewModel);

            dbStuffForHero.Proposer = proposer;
            dbStuffForHero.IsActive = true;

            _staffForHeroRepository.Save(dbStuffForHero);
            return RedirectToAction("Stuff");
        }

        public IActionResult RemoveStuff(long stuffId)
        {
            _staffForHeroRepository.Remove(stuffId);
            return RedirectToAction("Stuff");
        }

        public IActionResult Wonderful(long stuffId)
        {
            var stuff = _staffForHeroRepository.Get(stuffId);
            _payForActionService.CreatorEarnMoney(stuff.Proposer.Id, 10);

            return RedirectToAction("Stuff", "Stuff");
        }

        public IActionResult Awful(long stuffId)
        {
            var stuff = _staffForHeroRepository.Get(stuffId);

            _payForActionService.CreatorDislikeFine(stuff.Proposer.Id, TypesOfPayment.Fine);

            return RedirectToAction("Stuff", "Stuff");
        }
    }
}
