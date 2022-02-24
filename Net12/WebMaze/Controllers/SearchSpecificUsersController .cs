using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Controllers
{
    public class SearchSpecificUsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CellSuggestionWinner()
        {
            var myList = new List<string>() {"Bibi", "Tobi" };
            return View(myList);
        }
    }
}
