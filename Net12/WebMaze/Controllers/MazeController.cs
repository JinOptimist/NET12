using Microsoft.AspNetCore.Mvc;
using Net12.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult Tavern()
        {
            var model = new CellInfoViewModel();
            model.CanStep = true;
            model.ImageUrl = "/images/tavern.jpg";
            model.Desc = "Tavern, where you can spend your time.";
            model.Spec = "HP: Building";

            return View(model);
        }

        public IActionResult Walker()
        {
            var model = new CellInfoViewModel();
            model.CanStep = true;
            model.ImageUrl = "/images/walkerico.jpg";
            model.Desc = "A walking enemy of the hero who, when meeting the hero, deals damage to him.";
            model.Spec = "HP: Divine protection";

            return View(model);
        }
    }
}
