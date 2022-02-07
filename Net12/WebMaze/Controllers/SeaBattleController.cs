using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private SeaBattleFieldRepository _seaBattleFieldRepository;

        public SeaBattleController(SeaBattleService seaBattleService, SeaBattleDifficultRepository seaBattleDifficultRepository, SeaBattleFieldRepository seaBattleFieldRepository, IMapper mapper)
        {
            _seaBattleService = seaBattleService;
            _seaBattleDifficultRepository = seaBattleDifficultRepository;
            _seaBattleFieldRepository = seaBattleFieldRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult NewGame(long difficultId)
        {

            var difficult = _seaBattleDifficultRepository.Get(difficultId);

            var field = _seaBattleService.BuildField(difficult);
            _seaBattleFieldRepository.Save(field);

            return RedirectToAction("Game", new { id = field.Id });
        }

        public IActionResult Game(long id)
        {
            var field = _seaBattleFieldRepository.Get(id);
            var fieldViewModel = _mapper.Map<SeaBattleFieldViewModel>(field);



            return View(fieldViewModel);
        }
    }
}
