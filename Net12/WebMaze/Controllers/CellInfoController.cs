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
        [HttpGet]
        public IActionResult AddCell()
        {
            var model = new CellInfoViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddCell(CellInfoViewModel model)
        {
            return View("/Views/CellInfo/BaseCell.cshtml", model);
        }

        public IActionResult Goldmine()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.Url = "/images/goldmine.jpg";
            model.Desc = "Has 3 hp and gives you 1 coin every hit.";

            return View(model);
        }

        public IActionResult Trap()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.Url = "/images/trap.jfif";
            model.Desc = "Bad cell. Trap";

            return View(model);
        }
        public IActionResult Tavern()
        {
            var model = new CellInfoViewModel();
            model.CanStep = true;
            model.Url = "/images/tavern.jpg";
            model.Desc = "Tavern, where you can spend your time.";
            model.Spec = "HP: Building";

            return View(model);
        }

        public IActionResult Walker()
        {
            var model = new CellInfoViewModel();
            model.CanStep = true;
            model.Url = "/images/walkerico.jpg";
            model.Desc = "A walking enemy of the hero who, when meeting the hero, deals damage to him.";
            model.Spec = "HP: Divine protection";

            return View(model);
        }
        public IActionResult WeakWall()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.Url = "/imgYellowTeam/Weakwall.jpg";
            model.Desc = "After three hits, it breaks and instead of it, earth appears";
            model.ShortsDescriptions.Add("Strength = 3.");
            model.ShortsDescriptions.Add("Can be destroyed.");

            return View(model);
        }


        public IActionResult AgressiveTroll()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.Url = "/imgYellowTeam/Troll.jpg";
            model.Desc = "Aggressive troll tries to catch up with the hero";
            model.ShortsDescriptions.Add("Enemy.");

            return View(model);
        }
    }
}
