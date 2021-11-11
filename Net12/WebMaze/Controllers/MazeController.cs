using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models;

namespace WebMaze.Controllers
{
    public class CellInfoController : Controller
    {
        public IActionResult Trap()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.ImageUrl = "/images/trap.jfif";
            model.Desc = "Bad cell. Trap";

            return View(model);
        }
        public IActionResult Bed()
        {
            var model = new CellInfoViewModel();

            model.CanStep = true;
            model.ImageUrl = "/images/Bed.png";
            model.Desc = "Hero is getting tired while he is walking. You'll lost if Hero's fatigue reached a maximum value. Use the bed just not to be wasted.";
            model.Remainder = "Fatigue decreases to zero.";
            model.Use = "Use: once.";

            return View(model);
        }
        public IActionResult Goblin()
        {
            var model = new CellInfoViewModel();

            model.CanStep = true;
            model.ImageUrl = "/images/Goblin.png";
            model.Desc = "Goblin always rans off when Hero gets closer to him. Drive Goblin into a corner and grab him to get coins.";
            model.Remainder = "Hero will get great deal of money, but catch me first.";
            model.Use = "Use: once.";

            return View(model);
        }
    }
}
