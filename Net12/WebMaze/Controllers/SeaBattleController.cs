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
        private SeaBattleCellRepository _seaBattleCellRepository;
        private SeaBattleFieldRepository _seaBattleFieldRepository;

        public SeaBattleController(IMapper mapper,
                                    SeaBattleService seaBattleService,
                                    SeaBattleDifficultRepository seaBattleDifficultRepository,
                                    SeaBattleGameRepository seaBattleGameRepository,
                                    UserService userService,
                                    SeaBattleCellRepository seaBattleCellRepository,
                                    SeaBattleFieldRepository seaBattleFieldRepository)
        {
            _mapper = mapper;
            _seaBattleService = seaBattleService;
            _seaBattleDifficultRepository = seaBattleDifficultRepository;
            _seaBattleGameRepository = seaBattleGameRepository;
            _userService = userService;
            _seaBattleCellRepository = seaBattleCellRepository;
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
            var enemyCell = _seaBattleCellRepository.Get(id);

            enemyCell.Hit = true;

            _seaBattleCellRepository.Save(enemyCell);

            var enemyField = enemyCell.Field;

            if (!enemyField.Cells.Where(x => x.ShipHere && !x.Hit).Any())
            {
                _seaBattleGameRepository.Remove(enemyField.Game.Id);
                return RedirectToAction("WinGame");
            }

            _seaBattleService.FillNearKilledShips(enemyField);

            var myField = enemyCell.Field.Game.Fields.Where(x => !x.IsField).Single();

            if (!myField.Cells.Where(x => x.ShipHere && !x.Hit).Any())
            {
                _seaBattleGameRepository.Remove(myField.Game.Id);
                return RedirectToAction("LoseGame");
            }

            if (myField.LastHitToShip > 0)
            {
                _seaBattleService.DestroyShip(myField);
            }
            else
            {
                _seaBattleService.RandomHit(myField);
            }

            _seaBattleService.FillNearKilledShips(myField);

            return RedirectToAction("Game", new { id = _userService.GetCurrentUser().SeaBattleGame.Id });
        }

        public IActionResult WinGame()
        {
            return View();
        }

        public IActionResult LoseGame()
        {
            return View();
        }

        public IActionResult Remove()
        {
            _seaBattleGameRepository.Remove(_userService.GetCurrentUser().SeaBattleGame);
            return RedirectToAction("Index");
        }

    }
}
