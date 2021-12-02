using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class BugReportController : Controller
    {
        private UserRepository _userRepository;
        private BugReportRepository _bugReportRepository;
        private IMapper _mapper;
        private UserService _userService;

        public BugReportController(UserRepository userRepository,
            BugReportRepository bugReportRepository,
            IMapper mapper, UserService userService)
        {
            _userRepository = userRepository;
            _bugReportRepository = bugReportRepository;
            _mapper = mapper;
            _userService = userService;
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

        [Authorize]
        [HttpGet]
        public IActionResult AddBugReport()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddBugReport(BugReportViewModel bugReportViewModel)
        {
            var creater = _userService.GetCurrentUser();

            var dbBugReport = _mapper.Map<BugReport>(bugReportViewModel);
            dbBugReport.Creater = creater;
            dbBugReport.IsActive = true;

            _bugReportRepository.Save(dbBugReport);

            return RedirectToAction("BugReports", "BugReport");
        }

    }
}