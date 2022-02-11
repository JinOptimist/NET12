using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.SeaBattle;
using WebMaze.EfStuff.Repositories;
using WebMaze.EfStuff.Repositories.SeaBattle;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class SeaBattleController : Controller
    {
        private IMapper _mapper;
        private SeaBattleService _seaBattleService;
        private SeaBattleDifficultRepository _seaBattleDifficultRepository;
        private SeaBattleGameRepository _seaBattleGameRepository;
        private SeaBattleFieldRepository _seaBattleFieldRepository;
        public SeaBattleController(IMapper mapper,
                                    SeaBattleService seaBattleService,
                                    SeaBattleDifficultRepository seaBattleDifficultRepository,
                                    SeaBattleGameRepository seaBattleGameRepository,
                                    SeaBattleFieldRepository seaBattleFieldRepository)
        {
            _mapper = mapper;
            _seaBattleService = seaBattleService;
            _seaBattleDifficultRepository = seaBattleDifficultRepository;
            _seaBattleGameRepository = seaBattleGameRepository;
            _seaBattleFieldRepository = seaBattleFieldRepository;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult NewGame(long difficultId)
        {

            var difficult = new SeaBattleDifficult
            {
                Height = 12,
                Width = 12,
                FourSizeShip = 2,
                ThreeSizeShip = 3,
                TwoSizeShip = 4
            };

            var game = _seaBattleService.CreateGame(difficult);

            if (game == null)
            {
                return RedirectToAction("BoringError", "Home");
            }
            _seaBattleGameRepository.Save(game);


            return RedirectToAction("Game", new { id = game.Id });
        }

        public IActionResult Game(long id)
        {
            var game = _seaBattleGameRepository.Get(id);

            var gameViewModel = _mapper.Map<SeaBattleGameViewModel>(game);

            return View(gameViewModel);
        }

        //public IActionResult ClickOnCell(long id)
        //{
        //    var enemyCell = _seaBattleEnemyCellRepository.Get(id);
        //    var enemyField = enemyCell.Field;

        //    if (enemyCell.ShipHere)
        //    {
        //        enemyCell.Hit = true;
        //    }
        //    _seaBattleEnemyCellRepository.Save(enemyCell);

        //    return RedirectToAction("Game", new { id = _userService.GetCurrentUser().SeaBattleGame.Id });
        //}

    }
}
