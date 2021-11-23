using AutoMapper;
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
        private IMapper _mapper;

        public BugReportController(WebContext webContext,
            UserRepository userRepository, 
            BugReportRepository bugReportRepository,
            IMapper mapper)
        {
            _webContext = webContext;
            _userRepository = userRepository;
            _bugReportRepository = bugReportRepository;
            _mapper = mapper;
        }
        public IActionResult BugReports()
        {
            var bugReportViewModels = new List<BugReportViewModel>();
            
            bugReportViewModels = _bugReportRepository
                .GetAll()
                .Select(dbModel => _mapper.Map<BugReportViewModel>(dbModel))
                .ToList();

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

            var dbBugReport = _mapper.Map<BugReport>(bugReportViewModel);
            dbBugReport.Creater = creater;
            dbBugReport.IsActive = true;

            _bugReportRepository.Save(dbBugReport);

            return RedirectToAction("BugReports", "BugReport");
        }

    }
}