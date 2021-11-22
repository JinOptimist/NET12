using Microsoft.AspNetCore.Mvc;
using Net12.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;

namespace WebMaze.Controllers
{
    public class MazeController : Controller
    {
        private MazeDifficultRepository _mazeDifficultRepository;

        public MazeController(MazeDifficultRepository mazzeDifficultRepository)
        {
            _mazeDifficultRepository = mazzeDifficultRepository;
        }

        public IActionResult Index(int width, int height)
        {
            var mazeBuilder = new MazeBuilder();
            var maze = mazeBuilder.Build(width, height, 50, 100, true);
            return View(maze);
        }

        [HttpGet]
        public IActionResult AddMazeDifficult()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMazeDifficult(MazeDifficultProfileViewModel mazeDifficultProfileViewModel)
        {
            var dbMazeDifficult = new MazeDifficultProfile()
            {
                Name = mazeDifficultProfileViewModel.Name,
                Width = mazeDifficultProfileViewModel.Width,
                Height = mazeDifficultProfileViewModel.Height,
                HeroMoney = mazeDifficultProfileViewModel.HeroMoney,
                HeroMaxHp = mazeDifficultProfileViewModel.HeroMaxHp,
                HeroMaxFatigue = mazeDifficultProfileViewModel.HeroMaxFatigue,
                CoinCount = mazeDifficultProfileViewModel.CoinCount,
                IsActive = true,
                Creater = _mazeDifficultRepository.GetRandomUser(),
            };
            _mazeDifficultRepository.Save(dbMazeDifficult);

            return View();
        }

        public IActionResult ManageMazeDifficult()
        {
            var mazeDifficultProfileViewModels = new List<MazeDifficultProfileViewModel>();
            var suggestions = _mazeDifficultRepository.GetAll();
            foreach (var dbMazeDifficult in suggestions)
            {
                var mazeDifficultProfileViewModel = new MazeDifficultProfileViewModel();

                mazeDifficultProfileViewModel.MazeDifficultId = dbMazeDifficult.Id;
                mazeDifficultProfileViewModel.Name = dbMazeDifficult.Name;
                mazeDifficultProfileViewModel.Width = dbMazeDifficult.Width;
                mazeDifficultProfileViewModel.Height = dbMazeDifficult.Height;
                mazeDifficultProfileViewModel.HeroMoney = dbMazeDifficult.HeroMoney;
                mazeDifficultProfileViewModel.HeroMaxHp = dbMazeDifficult.HeroMaxHp;
                mazeDifficultProfileViewModel.HeroMaxFatigue = dbMazeDifficult.HeroMaxFatigue;
                mazeDifficultProfileViewModel.CoinCount = dbMazeDifficult.CoinCount;
                mazeDifficultProfileViewModel.Author = dbMazeDifficult.Creater.Name;

                mazeDifficultProfileViewModels.Add(mazeDifficultProfileViewModel);
            }

            return View(mazeDifficultProfileViewModels);
        }

        public IActionResult RemoveMazeDifficult(long Id)
        {
            _mazeDifficultRepository.Remove(Id);
            return RedirectToAction("ManageMazeDifficult", "Maze");
        }
    }
}
