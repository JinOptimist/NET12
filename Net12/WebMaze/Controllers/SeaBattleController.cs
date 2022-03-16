﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.SeaBattle;
using WebMaze.EfStuff.Repositories;
using WebMaze.EfStuff.Repositories.SeaBattle;
using WebMaze.Models;
using WebMaze.Services;
using WebMaze.SignalRHubs;

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
        private IHubContext<SeaBattleHub> _seaBattleHub;

        public SeaBattleController(IMapper mapper,
                                    SeaBattleService seaBattleService,
                                    SeaBattleDifficultRepository seaBattleDifficultRepository,
                                    SeaBattleGameRepository seaBattleGameRepository,
                                    UserService userService,
                                    SeaBattleCellRepository seaBattleCellRepository, 
                                    IHubContext<SeaBattleHub> seaBattleHub)
        {
            _mapper = mapper;
            _seaBattleService = seaBattleService;
            _seaBattleDifficultRepository = seaBattleDifficultRepository;
            _seaBattleGameRepository = seaBattleGameRepository;
            _userService = userService;
            _seaBattleCellRepository = seaBattleCellRepository;
            _seaBattleHub = seaBattleHub;
        }

        public IActionResult Index()
        {
            var indexViewModel = new SeaBattleIndexViewModel
            {
                SeaBattleDifficultViewModels = _seaBattleDifficultRepository.GetAll()
                                                                            .Select(x => _mapper.Map<SeaBattleDifficultViewModel>(x))
                                                                            .ToList()
            };

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

                _seaBattleGameRepository.Save(game);
            }
            else
            {
                game = user.SeaBattleGame;
            }

            _seaBattleService.StartTask(game.Id);

            return RedirectToAction("Game", new { id = game.Id });
        }
        public IActionResult ContinueGame()
        {
            var gameId = _userService.GetCurrentUser().SeaBattleGame.Id;
            _seaBattleService.StartTask(gameId);

            return RedirectToAction("Game", new { id = gameId });
        }

        public IActionResult Game(long id)
        {
            var game = _seaBattleGameRepository.Get(id);

            var myField = game.Fields.Single(x => !x.IsEnemyField);

            if (!myField.Cells.Any(x => x.IsShip && !x.Hit))
            {
                return RedirectToAction("LoseGame", new { gameId = id });
            }


            var gameViewModel = _mapper.Map<SeaBattleGameViewModel>(game);

            //заменяем целые ячейки кораблей на вражеском поле пустыми ячейками
            foreach (var cell in gameViewModel.EnemyField.Cells)
            {
                if (cell.IsShip && !cell.Hit)
                {
                    cell.IsShip = false;
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

            if (!enemyField.Cells.Any(x => x.IsShip && !x.Hit))
            {
                return RedirectToAction("WinGame", new { gameId = enemyField.Game.Id });
            }

            _seaBattleService.FillNearKilledShips(enemyField);

            //var myField = enemyCell.Field.Game.Fields.Single(x => !x.IsEnemyField);

            //_seaBattleService.EnemyTurn(myField);

            //if (!myField.Cells.Any(x => x.IsShip && !x.Hit))
            //{
            //    return RedirectToAction("LoseGame");
            //}

            return RedirectToAction("EnemyTurn", new { gameId = enemyField.Game.Id });
            //return RedirectToAction("Game", new { id = _userService.GetCurrentUser().SeaBattleGame.Id });
        }

        public IActionResult EnemyTurn(long gameId)
        {
            var myField = _seaBattleGameRepository.Get(gameId).Fields.Single(x => !x.IsEnemyField);

            _seaBattleService.EnemyTurn(myField);

            return RedirectToAction("Game", new { id = gameId });
        }

        public IActionResult WinGame(long gameId)
        {
            var seaBattleTask = SeaBattleService.SeaBattleTasks.First(x => x.Id == gameId);
            seaBattleTask.CancellationTokenSource.Cancel();

            lock (SeaBattleService.SeaBattleTasks)
            {
                SeaBattleService.SeaBattleTasks.Remove(seaBattleTask);
            }

            _seaBattleGameRepository.Remove(gameId);
            return View();
        }

        public IActionResult LoseGame(long gameId)
        {
            var seaBattleTask = SeaBattleService.SeaBattleTasks.First(x => x.Id == gameId);
            seaBattleTask.CancellationTokenSource.Cancel();

            lock (SeaBattleService.SeaBattleTasks)
            {
                SeaBattleService.SeaBattleTasks.Remove(seaBattleTask);
            }

            _seaBattleGameRepository.Remove(gameId);
            return View();
        }

        public void UserIsActive(long id)
        {
            var taskModel = SeaBattleService.SeaBattleTasks.First(x => x.Id == id);
            taskModel.UserIsActive = true;
        }


    }
}
