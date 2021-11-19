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
        public IActionResult VitalityPotion()
        {
            var model = new CellInfoViewModel();

            model.CanStep = true;
            model.Url = "/images/VitalityPotion.webp";
            model.Desc = "Увеличивает максимальное количество усталости на 5";
            return View(model);
        }
        public IActionResult Slime()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.Url = "/images/Slime.png";
            model.Desc = "Ходит по лабиринту и с некоторой вероятностью создает Монетку. При отсуствии пути перепрыгивает на случайную землю.";
            return View(model);
        }


        public IActionResult Geyser()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.Url = "/images/Geyser.jpg";
            model.Desc = "When the hero steps on a geyser , if there are earth - type cells around it , they change to the puddle type";
            
            return View(model);
        }

        public IActionResult Puddle()
        {
            var model = new CellInfoViewModel();

            model.CanStep = true;
            model.Url = "/images/Puddle.jpg";
            model.Desc = "There is the message 'wap - wap' on the screen when the hero steps on a puddle.";

            return View(model);
        }
    }
}
