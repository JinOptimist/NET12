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

        public IActionResult Goldmine()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.ImageUrl = "/images/goldmine.jpg";
            model.Desc = "Goldmine has 3 hp and gives you 1 coin every hit.";

            return View(model);
        }
    }
}
