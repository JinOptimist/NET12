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
        private SeaBattleEnemyFieldRepository _seaBattleEnemyFieldRepository;
        private SeaBattleEnemyCellRepository _seaBattleEnemyCellRepository;
        private UserService _userService;
        private SeaBattleGameRepository _seaBattleGameRepository;

        public SeaBattleController(SeaBattleService seaBattleService, SeaBattleDifficultRepository seaBattleDifficultRepository, IMapper mapper, SeaBattleEnemyFieldRepository seaBattleEnemyFieldRepository, SeaBattleEnemyCellRepository seaBattleEnemyCellRepository, UserService userService, SeaBattleGameRepository seaBattleGameRepository)
        {
            _seaBattleService = seaBattleService;
            _seaBattleDifficultRepository = seaBattleDifficultRepository;
            _mapper = mapper;
            _seaBattleEnemyFieldRepository = seaBattleEnemyFieldRepository;
            _seaBattleEnemyCellRepository = seaBattleEnemyCellRepository;
            _userService = userService;
            _seaBattleGameRepository = seaBattleGameRepository;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult NewGame(long difficultId)
        {

            var difficult = _seaBattleDifficultRepository.Get(difficultId);

            var game = _seaBattleService.CreateGame(difficult);
            _seaBattleGameRepository.Save(game);



            return RedirectToAction("Game", new { id = game.Id });
        }

        public IActionResult Game(long id)
        {
            var difficult = new SeaBattleDifficult
            {
                Height = 10,
                Width = 10,
                FourSizeShip = 2,
                ThreeSizeShip = 3,
                TwoSizeShip = 4
            };

            var field = _seaBattleService.BuildField(difficult);
            if (field == null)
            {
                return RedirectToAction("BoringError", "Home");
            }
            var fieldViewModel = _mapper.Map<SeaBattleFieldViewModel>(field);

            var fieldViewModels = new SeaBattleGameViewModel
            {
                MyField = fieldViewModel,
                EnemyField = fieldViewModel
            };

            return View(fieldViewModels);
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
