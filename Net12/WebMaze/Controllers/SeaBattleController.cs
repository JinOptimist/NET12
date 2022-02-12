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
        private Random _random = new Random();
        private IMapper _mapper;
        private SeaBattleService _seaBattleService;
        private SeaBattleDifficultRepository _seaBattleDifficultRepository;
        private SeaBattleGameRepository _seaBattleGameRepository;
        private UserService _userService;
        private SeaBattleEnemyCellRepository _seaBattleEnemyCellRepository;
        private SeaBattleMyFieldRepository _seaBattleMyFieldRepository;
        private SeaBattleMyCellRepository _seaBattleMyCellRepository;
        private SeaBattleEnemyFieldRepository _seaBattleEnemyFieldRepository;

        public SeaBattleController(IMapper mapper,
                                    SeaBattleService seaBattleService,
                                    SeaBattleDifficultRepository seaBattleDifficultRepository,
                                    SeaBattleGameRepository seaBattleGameRepository,
                                    UserService userService,
                                    SeaBattleEnemyCellRepository seaBattleEnemyCellRepository,
                                    SeaBattleMyCellRepository seaBattleMyCellRepository,
                                    SeaBattleMyFieldRepository seaBattleMyFieldRepository, 
                                    SeaBattleEnemyFieldRepository seaBattleEnemyFieldRepository)
        {
            _mapper = mapper;
            _seaBattleService = seaBattleService;
            _seaBattleDifficultRepository = seaBattleDifficultRepository;
            _seaBattleGameRepository = seaBattleGameRepository;
            _userService = userService;
            _seaBattleEnemyCellRepository = seaBattleEnemyCellRepository;
            _seaBattleMyCellRepository = seaBattleMyCellRepository;
            _seaBattleMyFieldRepository = seaBattleMyFieldRepository;
            _seaBattleEnemyFieldRepository = seaBattleEnemyFieldRepository;
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
            var user = _userService.GetCurrentUser();

            var game = new SeaBattleGame();

            if (user.SeaBattleGame == null)
            {
                game = _seaBattleService.CreateGame(difficult);

                if (game == null)
                {
                    return RedirectToAction("BoringError", "Home");
                }
                _seaBattleGameRepository.Save(game);
            }
            else
            {
                game = user.SeaBattleGame;
            }

            return RedirectToAction("Game", new { id = game.Id });
        }

        public IActionResult Game(long id)
        {
            var game = _seaBattleGameRepository.Get(id);

            var gameViewModel = _mapper.Map<SeaBattleGameViewModel>(game);

            foreach (var cell in gameViewModel.EnemyField.Cells)
            {
                if (cell.ShipHere && !cell.Hit)
                {
                    cell.ShipHere = false;
                }
            }

            return View(gameViewModel);
        }

        public IActionResult ClickOnCell(long id)
        {
            var enemyCell = _seaBattleEnemyCellRepository.Get(id);
            enemyCell.Hit = true;
            _seaBattleEnemyCellRepository.Save(enemyCell);

            var enemyField = enemyCell.Field;
            if (enemyField.Cells.Where(x=> x.ShipHere).Count() == enemyField.Cells.Where(x => x.ShipHere && x.Hit).Count())
            {
                _seaBattleGameRepository.Remove(enemyField.Game.Id);
                return RedirectToAction("Index");
                return RedirectToAction("WinGame");
            }

            var myFieldId = enemyCell.Field.Game.MyField.Id;
            var myField = _seaBattleMyFieldRepository.Get(myFieldId);


            var cell = new SeaBattleMyCell();
            do
            {
                var hitX = _random.Next(myField.Width);
                var hitY = _random.Next(myField.Height);

                cell = myField.Cells.Where(x => !x.Hit && x.X == hitX && x.Y == hitY).SingleOrDefault();
            }
            while (cell == null);

            cell.Hit = true;
            _seaBattleMyCellRepository.Save(cell);


            if (myField.Cells.Where(x => x.ShipHere).Count() == myField.Cells.Where(x => x.ShipHere && x.Hit).Count())
            {
                return RedirectToAction("LoseGame");
            }

            return RedirectToAction("Game", new { id = _userService.GetCurrentUser().SeaBattleGame.Id });
        }

    }
}
