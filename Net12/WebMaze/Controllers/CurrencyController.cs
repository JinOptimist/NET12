using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebMaze.Models.CurrencyDto;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class CurrencyController : Controller
    {
        private CurrenceService _currenceService;

        public CurrencyController(CurrenceService currenceService)
        {
            _currenceService = currenceService;
        }

        public async Task<ActionResult> Index()
        {
            var rates = await _currenceService.GetRates();

            return View(rates);
        }

        public async Task<IActionResult> GetRateById(int currencyId)
        {           
            var rate = await _currenceService.GetRateById(currencyId);

            return View(rate);
        }
    }
}
