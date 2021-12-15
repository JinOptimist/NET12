using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.EfStuff.DbModel;
using AutoMapper;
using System.Linq;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class GameDevicesController : Controller
    {
        private UserRepository _userRepository;
        private GameDevicesRepository _gameDevicesRepository;
        private IMapper _mapper;
        private UserService _userService;


        public GameDevicesController(UserRepository userRepository,
               GameDevicesRepository gameDevicesRepository, IMapper mapper, UserService userService)
        {
            _userRepository = userRepository;
            _gameDevicesRepository = gameDevicesRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public IActionResult GameDevices()
        {
            var gameDevicesViewModels = new List<GameDevicesViewModel>();
            gameDevicesViewModels = _gameDevicesRepository
                .GetAll()
                .Select(dbModel => _mapper.Map<GameDevicesViewModel>(dbModel))
                .ToList();

            return View(gameDevicesViewModels);
        }

        [HttpGet]
        public IActionResult AddGameDevices()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddGameDevices(GameDevicesViewModel gameDevicesViewModel)
        {
            var creater = _userService.GetCurrentUser();

            var dbGameDevices = _mapper.Map<GameDevices>(gameDevicesViewModel);
            dbGameDevices.Creater = creater;
            dbGameDevices.IsActive = true;

            _gameDevicesRepository.Save(dbGameDevices);

            return RedirectToAction("GameDevices", "GameDevices");
        }
    }


}
