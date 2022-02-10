using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.ThreeInRow;
using WebMaze.EfStuff.Repositories;
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
        private UserRepository _userRepository;
        private UserService _userService;
        private IMapper _mapper;

        public ThreeInRowController(ThreeInRowCellRepository threeInRowCellRepository,
                                    ThreeInRowGameFieldRepository threeInRowGameFieldRepository,
                                    ThreeInRowService threeInRowService,
                                    UserService userService,
                                    UserRepository userRepository,
                                    IMapper mapper)
        {
            _threeInRowCellRepository = threeInRowCellRepository;
            _threeInRowGameFieldRepository = threeInRowGameFieldRepository;
            _threeInRowService = threeInRowService;
            _userService = userService;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public IActionResult WelcomePage()
        {
            var userGames = _userRepository.Get(_userService.GetCurrentUser().Id).ThreeInRowGameFields.Where(m => m.IsActive).ToList();
            var userGamesViewModel = _mapper.Map<List<ThreeInRowGameFieldViewModel>>(userGames);

            return View(userGamesViewModel);
        }

        public IActionResult CreateGame()
        {
            var newGame = _threeInRowService.Build();
            _threeInRowGameFieldRepository.Save(newGame);

            return RedirectToAction("PlayGame", new { id = newGame.Id });
        }

        public IActionResult PlayGame(long id)
        {
            var gameField = _threeInRowGameFieldRepository.Get(id);
            var gameFieldViewModel = _mapper.Map<ThreeInRowGameFieldViewModel>(gameField);

            if (!gameField.Cells.Where(c=> c.Color == "none").Any())
            {
                gameField.IsActive = false;
                _threeInRowGameFieldRepository.Save(gameField);
                return RedirectToAction("GameOver", new { gameId = id });
            }

            return View(gameFieldViewModel);
        }

        public IActionResult GameOver(long gameId)
        {
            var gameField = _threeInRowGameFieldRepository.Get(gameId);
            var gameFieldViewModel = _mapper.Map<ThreeInRowGameFieldViewModel>(gameField);

            return View(gameFieldViewModel);
        }

        public IActionResult Step(long cellId, long gameId)
        {            
            _threeInRowService.Step(cellId, gameId);
            return RedirectToAction("PlayGame", new { id = gameId });
        }
    }
}