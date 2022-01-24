using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.Repositories.ThreeInRowRepositories;
using WebMaze.Models.ThreeInRow;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    [Authorize]
    public class ThreeInRowController : Controller
    {
        private ThreeInRowCellRepository _threeInRowCellRepository;
        private ThreeInRowGameFieldRepository _threeInRowGameFieldRepository;
        private ThreeInRowService _threeInRowService;
        private UserService _userService;
        private IMapper _mapper;

        public ThreeInRowController(ThreeInRowCellRepository threeInRowCellRepository,
                                    ThreeInRowGameFieldRepository threeInRowGameFieldRepository,
                                    ThreeInRowService threeInRowService,
                                    UserService userService,
                                    IMapper mapper)
        {
            _threeInRowCellRepository = threeInRowCellRepository;
            _threeInRowGameFieldRepository = threeInRowGameFieldRepository;
            _threeInRowService = threeInRowService;
            _userService = userService;
            _mapper = mapper;
        }

        public IActionResult WelcomePage()
        {
            var newGame = _threeInRowService.Build();
            _threeInRowGameFieldRepository.Save(newGame);

            return View();
        }

        public IActionResult Game()
        {
            var gameField = _threeInRowGameFieldRepository.Get(1);
            var gameFieldViewModel = _mapper.Map<ThreeInRowGameFieldViewModel>(gameField);

            return View(gameFieldViewModel);
        }

        public IActionResult ContinueGame()
        {
            var gameField = _threeInRowGameFieldRepository.Get(1);
            var gameFieldViewModel = _mapper.Map<ThreeInRowGameFieldViewModel>(gameField);

            return View(gameFieldViewModel);
        }

        public IActionResult Step(long Id)
        {
            _threeInRowService.Step(Id);
            return RedirectToAction("Game");
        }
    }
}