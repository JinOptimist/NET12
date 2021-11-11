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

        public IActionResult Fountain()
        {
            var model = new CellInfoViewModel();

            model.CanStep = true;
            model.ImageUrl = "/images/Fountain.jpg";
            model.Desc = "Fountain";
            //model.Desc[0] = "Hello";

            return View(model);
        }

        public IActionResult Coin()
        {
            var model = new CellInfoViewModel();

            model.CanStep = true;
            model.ImageUrl = "/images/Coin.jpg";
            model.Desc = "Coin";
            //model.Desc[0] = "Hello";

            return View(model);
        }

        

    }
}
