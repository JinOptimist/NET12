using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;

namespace WebMaze.Controllers
{
    public class FavoriteGamesController : Controller
    {
        private readonly FavGamesRepository _repository;
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;

        public FavoriteGamesController(FavGamesRepository repository, UserRepository userRepository, IMapper mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public IActionResult FavoriteGames()
        {
            //var GamesViewModels = new List<GameViewModel>();
            var GamesViewModels = _repository
               .GetAll()
               .Select(dbModel => _mapper.Map<GameViewModel>(dbModel))
               .ToList();
            /*var games = _webContext.FavGames.ToList();
            foreach (var dbGame in games)
            {
                var gameViewModel = new GameViewModel();
                gameViewModel.Name = dbGame.Name;
                gameViewModel.Genre = dbGame.Genre;
                gameViewModel.YearOfProd = dbGame.YearOfProd;
                gameViewModel.Desc = dbGame.Desc;
                gameViewModel.Rating = dbGame.Rating;
                gameViewModel.Username = dbGame.Creater.Name;
                gameViewModel.Age = dbGame.Creater.Age;
                GamesViewModels.Add(gameViewModel);
            }*/

            return View(GamesViewModels);
        }

        [HttpGet]
        public IActionResult AddGame()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddGame(GameViewModel gameViewMode)
        {
            /*var creater = _webContext.Users
                .Where(x => x.Name == gameViewMode.Username)
                .FirstOrDefault();*/

            var dbGame = _mapper.Map<Game>(gameViewModel);
            dbGame.Author = _userRepository.Get(gameViewModel.Author.Id);

            _repository.Save(dbGame);


            /*var dbGame = new Game()
            {
                Name = gameViewMode.Name,
                Genre = gameViewMode.Genre,
                YearOfProd = gameViewMode.YearOfProd,
                Desc = gameViewMode.Desc,
                Rating = gameViewMode.Rating,
                Creater = creater,
            };
            _webContext.FavGames.Add(dbGame);

            _webContext.SaveChanges();*/

            return RedirectToAction("FavoriteGames", "Home");
        }
    }
}
