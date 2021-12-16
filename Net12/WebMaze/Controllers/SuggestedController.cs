using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class SuggestedController : Controller
    {
        private NewCellSuggRepository _newCellSuggRepository;
        private UserRepository _userRepository;
        private SuggestedEnemysRepository _suggestedEnemysRepository;
        private IMapper _mapper;
        private UserService _userService;
        private readonly PayForActionService _payForActionService;

        public SuggestedController(UserRepository userRepository, ReviewRepository reviewRepository,
            SuggestedEnemysRepository suggestedEnemysRepository,
            IMapper mapper, NewCellSuggRepository newCellSuggRepository, UserService userService,
            PayForActionService payForActionService)
        {
            _userRepository = userRepository;
            _suggestedEnemysRepository = suggestedEnemysRepository;
            _mapper = mapper;
            _newCellSuggRepository = newCellSuggRepository;
            _userService = userService;
            _payForActionService = payForActionService;
        }

        public IActionResult SuggestedEnemys()
        {
            var suggestedEnemysViewModels = new List<SuggestedEnemysViewModel>();
            suggestedEnemysViewModels = _suggestedEnemysRepository
               .GetAll()
               .Select(dbModel => _mapper.Map<SuggestedEnemysViewModel>(dbModel))
               .ToList();

            return View(suggestedEnemysViewModels);
        }

        [Authorize]
        public IActionResult RemoveSuggestedEnemy(long suggestedEnemysId)
        {
            _suggestedEnemysRepository.Remove(suggestedEnemysId);
            return RedirectToAction($"{nameof(SuggestedController.SuggestedEnemys)}");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddSuggestedEnemy(long suggestedEnemysId)
        {
            var model = _mapper.Map<SuggestedEnemysViewModel>(_suggestedEnemysRepository.Get(suggestedEnemysId))
         ?? new SuggestedEnemysViewModel();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddSuggestedEnemy(SuggestedEnemysViewModel suggestedEnemysViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(suggestedEnemysViewModel);
            }

            if (!_payForActionService.Payment(200))
            {
                ModelState.AddModelError(string.Empty, "Not enought money to add enemy suggestion");
                return View(suggestedEnemysViewModel);
            }

            var creater = _userService.GetCurrentUser();

            var dbSuggestedEnemys = new SuggestedEnemys();
            dbSuggestedEnemys = _mapper.Map<SuggestedEnemys>(suggestedEnemysViewModel);
            dbSuggestedEnemys.Creater = creater;
            dbSuggestedEnemys.IsActive = true;

            _suggestedEnemysRepository.Save(dbSuggestedEnemys);

            return RedirectToAction($"{nameof(SuggestedController.SuggestedEnemys)}");
        }

        public IActionResult WonderfulSuggestedEnemy(long suggestedEnemysId)
        {
            var suggestedEnemys = _suggestedEnemysRepository.Get(suggestedEnemysId);
            _payForActionService.CreatorEarnMoney(suggestedEnemys.Creater.Id, 10);

            return RedirectToAction("SuggestedEnemys", "Suggested");
        }

        public IActionResult NewCellSugg()
        {
            var newCellSuggestionsViewModel = new List<NewCellSuggestionViewModel>();
            newCellSuggestionsViewModel = _newCellSuggRepository.GetAll()
                .Select(dbModel => _mapper.Map<NewCellSuggestionViewModel>(dbModel))
                .ToList();

            return View(newCellSuggestionsViewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddNewCellSugg(long Id)
        {
            var model = _mapper.Map<NewCellSuggestionViewModel>(_newCellSuggRepository.Get(Id))
           ?? new NewCellSuggestionViewModel();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddNewCellSugg(NewCellSuggestionViewModel newCellSuggestionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(newCellSuggestionViewModel);
            }

            if (!_payForActionService.Payment(200))
            {
                ModelState.AddModelError(string.Empty, "Not enought money to add cell suggestion");
                return View(newCellSuggestionViewModel);
            }

            var creater = _userRepository.GetRandomUser();

            var NewCS = new NewCellSuggestion();

            NewCS = _mapper.Map<NewCellSuggestion>(newCellSuggestionViewModel);
            NewCS.Creater = creater;
            NewCS.IsActive = true;

            _newCellSuggRepository.Save(NewCS);
            return RedirectToAction($"{nameof(SuggestedController.NewCellSugg)}");
        }

        [Authorize]
        public IActionResult RemoveNewCellSuggestion(long id)
        {
            _newCellSuggRepository.Remove(id);
            return RedirectToAction($"{nameof(SuggestedController.NewCellSugg)}");
        }

        public IActionResult WonderfulNewCellSuggestion(long newCellSuggestionId)
        {
            var newCellSuggestion = _newCellSuggRepository.Get(newCellSuggestionId);
            _payForActionService.CreatorEarnMoney(newCellSuggestion.Creater.Id, 10);

            return RedirectToAction("NewCellSugg", "Suggested");
        }
    }
}
