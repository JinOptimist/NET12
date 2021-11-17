using Microsoft.AspNetCore.Mvc;
using Net12.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models;

namespace WebMaze.Controllers
{
    public class MazeController : Controller
    {
      
        public IActionResult Index(int width, int height)
        {
            var mazeBuilder = new MazeBuilder();
            var maze = mazeBuilder.Build(width, height, 50, 100, true);
            return View(maze);
        }
        public IActionResult Healer()
        {
            var model = new CellInfoViewModel();

            model.CanStep = true;
            model.Url = "/imgYellowTeam/healer.jpg";
            model.Desc = "A kind doctor will help you to improve your health. But the money will take half of all." +
                " Come in with one coin in hand.";
            model.ShortsDescriptions.Add("Cuts money in half.");
            model.ShortsDescriptions.Add("Increases health to maximum.");
            return View(model);
        }
        public IActionResult Wallworm()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.Url = "/imgYellowTeam/worm.jpg";
            model.Desc = "This worm lives in the wall and will help you make a hole in the wall. " +
                "But do not yawn, he eats not only a wall but also a gold mine.";
            model.ShortsDescriptions.Add("Breaks down the walls.");
            model.ShortsDescriptions.Add("Destroys gold mines.");
            model.ShortsDescriptions.Add("Creates weak walls");
            return View(model);
        }
    }
}
