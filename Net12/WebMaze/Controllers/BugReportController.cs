using Microsoft.AspNetCore.Mvc;
using Net12.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.Models;

namespace WebMaze.Controllers
{
    public class BugReportController : Controller
    {
        private WebContext _webContext;

        public BugReportController(WebContext webContext)
        {
            _webContext = webContext;
        }
        public IActionResult Reports()
        {
            var bugReportViewModels = new List<BugReportViewModel>();
            var BugReports = _webContext.BugReports.ToList();
            foreach (var dbBugReport in BugReports)
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
            var creater = _webContext
                .Users
                .OrderBy(x => x.Id).
                FirstOrDefault();
            var dbBugReport = new BugReport()
            {
                Creater = creater,
                Description = bugReportViewModel.Description
            };
            _webContext.BugReports.Add(dbBugReport);
            _webContext.SaveChanges();

            return RedirectToAction("Reports", "BugReport");
        }

    }
}
