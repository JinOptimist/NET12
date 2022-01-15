using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;
using WebMaze.SignalRHubs;

namespace WebMaze.Controllers
{
    public class ZumaGameController : Controller
    {
        private ZumaGameService _zumaGameService;
        private ZumaGameFieldRepository _zumaGameFieldRepository;
        private ZumaGameDifficultRepository _zumaGameDifficultRepository;
        private IMapper _mapper;
        private IHubContext<ChatHub> _chatHub;
        private UserService _userService;

        public ZumaGameController(ZumaGameService zumaGameService, ZumaGameFieldRepository zumaGameFieldRepository, IMapper mapper, IHubContext<ChatHub> chatHub, UserService userService, ZumaGameDifficultRepository zumaGameDifficultRepository)
        {
            _zumaGameService = zumaGameService;
            _zumaGameFieldRepository = zumaGameFieldRepository;
            _mapper = mapper;
            _chatHub = chatHub;
            _userService = userService;
            _zumaGameDifficultRepository = zumaGameDifficultRepository;
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

            if (_userService.RemoveCoins(difficult.Price))
            {

                var field = _zumaGameService.BuildField(difficult.Width, difficult.Height, difficult.ColorCount);
                _zumaGameFieldRepository.Save(field);

                return RedirectToAction("Game", new { id = field.Id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult ContinueGame()
        {
            return RedirectToAction("Game", new { id = _userService.GetCurrentUser().ZumaGameField.Id });
        }

        public IActionResult Game(long id)
        {
            var field = _zumaGameFieldRepository.Get(id);

            if (field.Cells.Count() > 1)
            {

                if (_zumaGameService.ClickedCell(field))
                {
                    var fieldViewModel = _mapper.Map<ZumaGameFieldViewModel>(field);

                    return View(fieldViewModel);
                }
                else
                {
                    return RedirectToAction("LoseGame", new { id });
                }
            }
            else
            {
                return RedirectToAction("WinGame", new { id });
            }
        }

        public IActionResult ClickOnCell(long Id)
        {

            _zumaGameService.RemoveCellsNearClicked(Id);

            return RedirectToAction("Game", new { id = _userService.GetCurrentUser().ZumaGameField.Id });
        }

        public IActionResult WinGame(long id)
        {
            var model = new ZumaGameScoreViewModel
            {
                Score = _zumaGameFieldRepository.Get(id).Score
            };

            _userService.AddCoins(model.Score);
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

            _userService.AddCoins(model.Score / 3);
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
    }
}
