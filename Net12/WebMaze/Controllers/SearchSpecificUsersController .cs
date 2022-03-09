using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebMaze.EfStuff.Repositories;

namespace WebMaze.Controllers
{
    public class SearchSpecificUsersController : Controller
    {
        private SearchSpecificUsersRepository _searchSpecificUsersRepository;
        public SearchSpecificUsersController(SearchSpecificUsersRepository searchSpecificUsersRepository)
        {
            _searchSpecificUsersRepository = searchSpecificUsersRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CellSuggestionWinner()
        {
            var myList = _searchSpecificUsersRepository.GetWonUsersInNewCellSugg();

            return View(myList);
        }
    }
}
