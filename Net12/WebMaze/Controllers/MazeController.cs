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

        public IActionResult VitalityPotion()
        {
            var model = new CellInfoViewModel();

            model.CanStep = true;
            model.ImageUrl = "/images/VitalityPotion.webp";
            model.Desc = "Увеличивает максимальное количество усталости на 5";
            return View(model);
        }
        public IActionResult Slime()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.ImageUrl = "/images/Slime.png";
            model.Desc = "Ходит по лабиринту и с некоторой вероятностью создает Монетку. При отсуствии пути перепрыгивает на случайную землю.";
            return View(model);
        }

    }
}
