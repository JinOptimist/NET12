using Microsoft.AspNetCore.Mvc;
using Net12.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.Models;

namespace WebMaze.Controllers
{
    public class MazeController : Controller
    {
        private WebContext _webContext;

        public MazeController(WebContext webContext)
        {
            _webContext = webContext;
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
                HeroMoney = mazeDifficultProfileViewModel.HeroMoney,
                HeroMaxHp = mazeDifficultProfileViewModel.HeroMaxHp,
                HeroMaxFatigue = mazeDifficultProfileViewModel.HeroMaxFatigue,
                CoinCount = mazeDifficultProfileViewModel.CoinCount
            };
            _webContext.MazeDifficultProfiles.Add(dbMazeDifficult);

            _webContext.SaveChanges();

            return View();
        }

    }
}
