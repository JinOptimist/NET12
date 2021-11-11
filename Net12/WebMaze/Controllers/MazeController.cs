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

        public IActionResult Geyser()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.ImageUrl = "/images/Geyser.jpg";
            model.Desc = "When the hero steps on a geyser , if there are earth - type cells around it , they change to the puddle type";

            return View(model);
        }

        public IActionResult Puddle()
        {
            var model = new CellInfoViewModel();

            model.CanStep = true;
            model.ImageUrl = "/images/Puddle.jpg";
            model.Desc = "There is the message 'wap - wap' on the screen when the hero steps on a puddle.";

            return View(model);
        }
    }
}
