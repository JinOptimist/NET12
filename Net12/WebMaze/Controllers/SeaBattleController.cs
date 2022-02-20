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
        private UserService _userService;
        private SeaBattleCellRepository _seaBattleCellRepository;

        public SeaBattleController(IMapper mapper,
                                    SeaBattleService seaBattleService,
                                    SeaBattleDifficultRepository seaBattleDifficultRepository,
                                    SeaBattleGameRepository seaBattleGameRepository,
                                    UserService userService,
                                    SeaBattleCellRepository seaBattleCellRepository)
        {
            _mapper = mapper;
            _seaBattleService = seaBattleService;
            _seaBattleDifficultRepository = seaBattleDifficultRepository;
            _seaBattleGameRepository = seaBattleGameRepository;
            _userService = userService;
            _seaBattleCellRepository = seaBattleCellRepository;
        }

        public IActionResult Index()
        {
            var indexViewModel = new SeaBattleIndexViewModel();

            indexViewModel.SeaBattleDifficultViewModels = _seaBattleDifficultRepository
                .GetAll()
                .Select(x => _mapper.Map<SeaBattleDifficultViewModel>(x))
                .ToList();

            if (_userService.GetCurrentUser().SeaBattleGame != null)
            {
                indexViewModel.IsContinue = true;
            }

            return View(indexViewModel);
        }

        public IActionResult NewGame(long difficultId)
        {
            var user = _userService.GetCurrentUser();
            var difficult = _seaBattleDifficultRepository.Get(difficultId);
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
        public IActionResult ContinueGame()
        {
            return RedirectToAction("Game", new { id = _userService.GetCurrentUser().SeaBattleGame.Id });
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
                return RedirectToAction("WinGame");
            }

            _seaBattleService.FillNearKilledShips(enemyField);

            var myField = enemyCell.Field.Game.Fields.Where(x => !x.IsField).Single();

            if (!myField.Cells.Where(x => x.ShipHere && !x.Hit).Any())
            {
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
            _seaBattleGameRepository.Remove(_userService.GetCurrentUser().SeaBattleGame.Id);
            return View();
        }

        public IActionResult LoseGame()
        {
            _seaBattleGameRepository.Remove(_userService.GetCurrentUser().SeaBattleGame.Id);
            return View();
        }

        public IActionResult Remove()
        {
            _seaBattleGameRepository.Remove(_userService.GetCurrentUser().SeaBattleGame);
            return RedirectToAction("Index");
        }

    }
}
