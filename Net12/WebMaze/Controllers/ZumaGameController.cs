using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;
using WebMaze.SignalRHubs;

namespace WebMaze.Controllers
{
    public class ZumaGameController : Controller
    {
        private ZumaGameFieldBuilder _zumaGameFieldBuilder;
        private ZumaGameFieldRepository _zumaGameFieldRepository;
        private ZumaGameCellRepository _zumaGameCellRepository;
        private ZumaGameDifficultRepository _zumaGameDifficultRepository;
        private IMapper _mapper;
        private IHubContext<ChatHub> _chatHub;
        private UserService _userService;
        private UserRepository _userRepository;

        public ZumaGameController(ZumaGameFieldBuilder zumaGameFieldBuilder, ZumaGameFieldRepository zumaGameFieldRepository, IMapper mapper, ZumaGameCellRepository zumaGameCellRepository, IHubContext<ChatHub> chatHub, UserService userService, ZumaGameDifficultRepository zumaGameDifficultRepository, UserRepository userRepository)
        {
            _zumaGameFieldBuilder = zumaGameFieldBuilder;
            _zumaGameFieldRepository = zumaGameFieldRepository;
            _mapper = mapper;
            _zumaGameCellRepository = zumaGameCellRepository;
            _chatHub = chatHub;
            _userService = userService;
            _zumaGameDifficultRepository = zumaGameDifficultRepository;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            var zumaGameDifficultViewModels = _zumaGameDifficultRepository.GetAll()
                .Select(x => _mapper.Map<ZumaGameDifficultViewModel>(x)).ToList();

            var indexViewModel = new ZumaGameIndexViewModel
            {
                ViewModels = zumaGameDifficultViewModels
            };

            if (_userService.GetCurrentUser().ZumaGameField != null)
            {
                indexViewModel.Continue = true;
            }

            indexViewModel.Coins = _userService.GetCurrentUser().Coins;

            return View(indexViewModel);
        }

        public IActionResult NewGame(long difficultId)
        {
            var difficult = _zumaGameDifficultRepository.Get(difficultId);
            var gamer = _userService.GetCurrentUser();

            gamer.Coins -= difficult.Price;

            if (gamer.Coins < 0)
            {
                return RedirectToAction("Error");
            }
            else
            {
                _userRepository.Save(gamer);
            }

            var field = _zumaGameFieldBuilder.Build(difficult.Width, difficult.Height, difficult.ColorCount);
            _zumaGameFieldRepository.Save(field);

            return RedirectToAction("Game", new { id = field.Id });
        }

        public IActionResult ContinueGame()
        {
            return RedirectToAction("Game", new { id = _userService.GetCurrentUser().ZumaGameField.Id });
        }

        public IActionResult Game(long id)
        {
            var field = _zumaGameFieldRepository.Get(id);
            var loseGame = true;

            if (field.Cells.Count() > 1)
            {
                foreach (var currentCell in field.Cells)
                {
                    if (field.Cells
                        .Where(cell => (cell.X == currentCell.X && Math.Abs(cell.Y - currentCell.Y) == 1
                        || Math.Abs(cell.X - currentCell.X) == 1 && cell.Y == currentCell.Y)
                        && cell.Color == currentCell.Color)
                        .Any())
                    {
                        loseGame = false;
                        break;
                    }
                }

                if (loseGame)
                {
                    return RedirectToAction("LoseGame", new { id });
                }
                else
                {
                    var fieldViewModel = _mapper.Map<ZumaGameFieldViewModel>(field);

                    return View(fieldViewModel);
                }
            }
            else
            {
                return RedirectToAction("WinGame", new { id });
            }
        }

        public IActionResult ClickOnCell(long Id)
        {
            var cell = _zumaGameCellRepository.Get(Id);
            var field = cell.Field;
            var cells = field.Cells;

            var getNear = _zumaGameCellRepository.GetNear(cell);

            if (getNear.Count() > 1)
            {
                foreach (var replaceCell in getNear)
                {
                    var replaceCells = cells.Where(db => db.Y < replaceCell.Y && db.X == replaceCell.X).ToList();

                    replaceCells.ForEach(x => x.Y++);

                    _zumaGameCellRepository.Remove(replaceCell);

                    _zumaGameCellRepository.UpdateCells(replaceCells);
                }

                for (int i = field.Width - 1; i >= 0; i--)
                {
                    var column = cells.Where(x => x.X == i).ToList();

                    if (column.Count() == 0)
                    {
                        for (int l = i + 1; l < field.Width; l++)
                        {
                            var cellsOffset = cells.Where(cell => cell.X == l).ToList();

                            cellsOffset.ForEach(cell => cell.X--);

                            _zumaGameCellRepository.UpdateCells(cellsOffset);
                        }
                    }
                }

                field.Score += (int)Math.Round(getNear.Count * Math.Pow(1.5, field.ColorCount - 2));
                _zumaGameFieldRepository.Save(field);
            }

            return RedirectToAction("Game", new { id = field.Id });
        }

        public IActionResult WinGame(long id)
        {
            var model = new ZumaGameScoreViewModel
            {
                Score = _zumaGameFieldRepository.Get(id).Score
            };

            var gamer = _userService.GetCurrentUser();
            gamer.Coins += model.Score;
            _userRepository.Save(gamer);

            _zumaGameFieldRepository.Remove(id);
            _chatHub.Clients.All.SendAsync("ZumaGameWin", _userService.GetCurrentUser().Name);
            return View(model);
        }

        public IActionResult LoseGame(long id)
        {
            var model = new ZumaGameScoreViewModel
            {
                Score = _zumaGameFieldRepository.Get(id).Score
            };

            var gamer = _userService.GetCurrentUser();
            gamer.Coins += model.Score / 3;
            _userRepository.Save(gamer);

            _zumaGameFieldRepository.Remove(id);
            _chatHub.Clients.All.SendAsync("ZumaGameLose", _userService.GetCurrentUser().Name);
            return View(model);
        }

        [HttpGet]
        public IActionResult AddDifficult(long difficultId)
        {
            var model = _mapper.Map<ZumaGameDifficultViewModel>(_zumaGameDifficultRepository.Get(difficultId))
                ?? new ZumaGameDifficultViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddDifficult(ZumaGameDifficultViewModel zumaGameDifficultViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(zumaGameDifficultViewModel);
            }

            var author = _userService.GetCurrentUser();

            var dbZumaGameDifficult = _mapper.Map<ZumaGameDifficult>(zumaGameDifficultViewModel);

            dbZumaGameDifficult.Author = author;
            dbZumaGameDifficult.IsActive = true;
            dbZumaGameDifficult.Price = (dbZumaGameDifficult.Height * dbZumaGameDifficult.Width) / (dbZumaGameDifficult.ColorCount / 2);

            _zumaGameDifficultRepository.Save(dbZumaGameDifficult);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveDifficult(long difficultId)
        {
            _zumaGameDifficultRepository.Remove(difficultId);
            return RedirectToAction("Index");
        }

        public IActionResult GetColors()
        {
            var colors = Enum.GetValues(typeof(ZumaGameColors))
                .Cast<ZumaGameColors>()
                .Select(x => x.ToString())
                .ToList();

            return Json(JsonSerializer.Serialize(colors));
        }
    }
}
