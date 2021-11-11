﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult WeakWall()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.ImageUrl = "/images/Weakwall.jpg";
            model.Desc = "After three hits, it breaks and instead of it, earth appears";
            model.ShortDesc[0] = "Strength = 3.";
            model.ShortDesc[1] = "Can be destroyed.";

            return View(model);
        }
       

        public IActionResult AgressiveTroll()
        {
            var model = new CellInfoViewModel();

            model.CanStep = false;
            model.ImageUrl = "/images/Troll.jpg";
            model.Desc = "Aggressive troll tries to catch up with the hero";
            model.ShortDesc[0] = "Enemy.";

            return View(model);

        }
       
    }
}
