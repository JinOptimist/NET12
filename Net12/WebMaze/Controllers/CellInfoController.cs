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

        public IActionResult StoresAndAddresses()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.Url = "/images/trap.jfif";
            model.Desc = "Bad cell. Trap";

            return View(model);
        }
    }
}
