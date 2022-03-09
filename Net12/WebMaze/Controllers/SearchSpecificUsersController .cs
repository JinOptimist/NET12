using Microsoft.AspNetCore.Mvc;
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
            var CellSuggWinners = _searchSpecificUsersRepository.GetWonUsersInNewCellSugg();

            return View(CellSuggWinners);
        }
    }
}
