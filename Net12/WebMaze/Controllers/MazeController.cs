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

            return View(model);
        }
        public IActionResult WeakWall()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.ImageUrl = "/imgYellowTeam/Weakwall.jpg";
            model.Desc = "After three hits, it breaks and instead of it, earth appears";
            model.ShortsDescriptions.Add("Strength = 3.");
            model.ShortsDescriptions.Add("Can be destroyed.");

            return View(model);
        }


        public IActionResult AgressiveTroll()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.ImageUrl = "/imgYellowTeam/Troll.jpg";
            model.Desc = "Aggressive troll tries to catch up with the hero";
            model.ShortsDescriptions.Add("Enemy.");

            return View(model);

        }

    }
}
