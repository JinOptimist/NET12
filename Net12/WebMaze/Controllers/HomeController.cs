using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.Models;

namespace WebMaze.Controllers
{
    public class HomeController : Controller
    {
        private WebContext _webContext;

        public HomeController(WebContext webContext)
        {
            _webContext = webContext;
        }

        public IActionResult Index()
        {
            var userViewModels = new List<UserViewModel>();
            foreach (var dbUser in _webContext.Users)
            {
                var userViewModel = new UserViewModel();
                userViewModel.UserName = dbUser.Name;
                userViewModel.Coins = dbUser.Coins;
                userViewModels.Add(userViewModel);
            }

            //var userViewModels2 = _webContext.Users.Select(
            //    dbModel => new UserViewModel { 
            //        UserName = dbModel.Name, 
            //        Coins = dbModel.Coins 
            //    });

            return View(userViewModels);
        }

        public IActionResult Reports()
        {
            var bugReportViewModels = new List<BugReportViewModel>();
            foreach (var dbBugReport in _webContext.BugReports)
            {
                var bugReportViewModel = new BugReportViewModel();
                bugReportViewModel.UserName = dbBugReport.Name;
                bugReportViewModel.Description = dbBugReport.Description;
                bugReportViewModels.Add(bugReportViewModel);

            }
            return View(bugReportViewModels);
        }

        [HttpGet]
        public IActionResult AddBugReport()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBugReport(BugReportViewModel bugReportViewModel)
        {

            var dbBugReport = new BugReport()
            {
                Name = bugReportViewModel.UserName,
                Description = bugReportViewModel.Description
            };
            _webContext.BugReports.Add(dbBugReport);
            _webContext.SaveChanges();

            return RedirectToAction("Reports", "Home");
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(UserViewModel userViewMode)
        {
            var dbUser = new User()
            {
                Name = userViewMode.UserName,
                Coins = userViewMode.Coins,
                Age = DateTime.Now.Second % 10 + 20
            };
            _webContext.Users.Add(dbUser);

            _webContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Time()
        {
            var smile = DateTime.Now.Second;
            return View(smile);
        }

        [HttpGet]
        public IActionResult Sum()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Sum(int x, int y)
        {
            var model = x + y;
            return View(model);
        }
    }
}
