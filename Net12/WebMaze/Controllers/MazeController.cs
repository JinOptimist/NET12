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
        public IActionResult Tavern()
        {
            var model = new CellInfoViewModel();
   
            model.CanStep = true;
            model.ImageUrl = "img/tavern.jpg";
            model.Desc = "Tavern, where you can spend your time.";
            model.Spec = "HP: Building";

            return View(model);
        }
    }
}
