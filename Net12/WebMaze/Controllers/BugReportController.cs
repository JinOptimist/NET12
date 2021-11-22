using Microsoft.AspNetCore.Mvc;
using Net12.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;

namespace WebMaze.Controllers
{
    public class BugReportController : Controller
    {
        private WebContext _webContext;
        private UserRepository _userRepository;
        private BugReportRepository _bugReportRepository;

        public BugReportController(WebContext webContext,
            UserRepository userRepository, BugReportRepository bugReportRepository)
        {
            _webContext = webContext;
            _userRepository = userRepository;
            _bugReportRepository = bugReportRepository;
        }
        public IActionResult Reports()
        {
            var bugReportViewModels = new List<BugReportViewModel>();
            foreach (var dbBugReport in _bugReportRepository.GetAllBugReports())
            {
                var bugReportViewModel = new BugReportViewModel();
                bugReportViewModel.UserName = dbBugReport.Creater.Name;
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
            var creater = _userRepository.GetRandomUser();
            var dbBugReport = new BugReport()
            {
                Creater = creater,
                Description = bugReportViewModel.Description
            };
            _bugReportRepository.dbAddBugReport(dbBugReport);

            return RedirectToAction("Reports", "BugReport");
        }

    }
}
