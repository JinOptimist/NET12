//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;
//using WebMaze.EfStuff;
//using WebMaze.EfStuff.DbModel;
//using WebMaze.Models;

//namespace WebMaze.Controllers
//{
//    public class StaffForHeroController : Controller
//    {
//        private WebContext _webContext;

//        public StaffForHeroController(WebContext webContext)
//        {
//            _webContext = webContext;
//        }

//        public IActionResult Index()
//        {
//            var staffsForHero = new List<StaffForHeroViewModel>();
//            foreach (var dbStaff in _webContext.StuffsForHero)
//            {
//                var staffForHeroViewModel = new StaffForHeroViewModel();
//                staffForHeroViewModel.Name = dbStaff.Name;
//                staffForHeroViewModel.Description = dbStaff.Description;
//                staffForHeroViewModel.PictureLink = dbStaff.PictureLink;
//                staffForHeroViewModel.Price = dbStaff.Price;
//                staffsForHero.Add(staffForHeroViewModel);
//            }

//            //var staffsForHero = _webContext.StuffsForHero.Select(dbModel => new StaffForHeroViewModel {
//            //    Name = dbModel.Name, Description = dbModel.Description, 
//            //    PictureLink = dbModel.PictureLink, Price = dbModel.Price});

//            return View(staffsForHero);
//        }
//    }
//}
