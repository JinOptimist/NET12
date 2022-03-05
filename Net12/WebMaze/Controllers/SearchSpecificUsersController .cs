using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

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
            var myList = new List<User>();
            myList

            return View(myList);
        }
    }
}
