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
                return RedirectToAction("Index");
                return RedirectToAction("WinGame");
            }

            _seaBattleService.FillNearKilledShips(enemyField);

            var myField = enemyCell.Field.Game.Fields.Where(x => !x.IsField).Single();

            if (myField.LastHitToShip > 0)
            {
                var lastHitCell = _seaBattleCellRepository.Get(myField.LastHitToShip);

                var getNearCells = myField.Cells
                    .Where(cell => (cell.X == lastHitCell.X && Math.Abs(cell.Y - lastHitCell.Y) == 1
                    || Math.Abs(cell.X - lastHitCell.X) == 1 && cell.Y == lastHitCell.Y)
                    && !cell.Field.IsField)
                    .ToList();

                if (getNearCells.Where(x => x.ShipHere && x.Hit).Any())
                {
                    var secondHit = getNearCells.Where(x => x.ShipHere && x.Hit).First();
                    if (lastHitCell.X == secondHit.X)
                    {
                        //vertical
                        for (int i = 1; i < lastHitCell.ShipLength; i++)
                        {
                            var cellsToHit = myField.Cells
                                .Where(cell => (cell.X == lastHitCell.X && cell.Y - lastHitCell.Y == i)
                                                && !cell.Field.IsField
                                                && !cell.Hit).Single();

                        }
                    }
                    if (lastHitCell.Y == secondHit.Y)
                    {
                        //horizontal
                    }
                }
                else
                {
                    if (getNearCells.Where(x => !x.Hit).Any())
                    {
                        var cellToHit = getNearCells[_random.Next(getNearCells.Count())];
                        cellToHit.Hit = true;
                        _seaBattleCellRepository.Save(cellToHit);
                    }
                }
            }
            else
            {
                var cell = new SeaBattleCell();
                do
                {
                    var hitX = _random.Next(myField.Width);
                    var hitY = _random.Next(myField.Height);

                    cell = myField.Cells.Where(x => !x.Hit && x.X == hitX && x.Y == hitY).SingleOrDefault();
                }
                while (cell == null);

                cell.Hit = true;

                _seaBattleCellRepository.Save(cell);

                if (cell.ShipHere)
                {
                    myField.LastHitToShip = cell.Id;
                    _seaBattleFieldRepository.Save(myField);
                }

            }

            if (!myField.Cells.Where(x => x.ShipHere && !x.Hit).Any())
            {
                return RedirectToAction("LoseGame");
            }

            _seaBattleService.FillNearKilledShips(myField);

            return RedirectToAction("Game", new { id = _userService.GetCurrentUser().SeaBattleGame.Id });
        }

        public IActionResult Remove()
        {
            _seaBattleGameRepository.Remove(_userService.GetCurrentUser().SeaBattleGame);
            return RedirectToAction("Index");
        }

    }
}
