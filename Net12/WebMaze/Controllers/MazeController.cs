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
            model.ImageUrl = "data:image/jpeg;base64,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";
            model.Desc = "Bad cell. Trap";

            return View(model);
        }
        public IActionResult HealerYT()
        {
            var model = new CellInfoViewModel();

            model.CanStep = true;
            model.ImageUrl = "/img/healer.jpg";
            model.Desc = "A kind doctor will help you to improve your health. But the money will take half of all. Come in with one coin in hand.";
            model.ShortDesc[0] = "Cuts money in half.";
            model.ShortDesc[1] = "Increases health to maximum.";
            return View(model);
        }
        public IActionResult Wallworm()
        {
            var model = new CellInfoViewModel();

            model.CanStep = true;
            model.ImageUrl = "/img/worm.jpg";
            model.Desc = "This worm lives in the wall and will help you make a hole in the wall. But do not yawn, he eats not only a wall but also a gold mine.";
            return View(model);
        }
    }
}
