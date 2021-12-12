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
        private readonly PayForActionService _payForActionService;

        public BugReportController(UserRepository userRepository,
            BugReportRepository bugReportRepository,
            IMapper mapper, UserService userService,
            PayForActionService payForActionService)
        {
            _userRepository = userRepository;
            _bugReportRepository = bugReportRepository;
            _mapper = mapper;
            _userService = userService;
            _payForActionService = payForActionService;
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
            if (!ModelState.IsValid)
            {
                return View(bugReportViewModel);
            }


            if (!_payForActionService.Payment(200))
            {
                ModelState.AddModelError(string.Empty, "Not enought money to add bugg report");
                return View(bugReportViewModel);
            }

            var creater = _userService.GetCurrentUser();

            var dbBugReport = _mapper.Map<BugReport>(bugReportViewModel);
            dbBugReport.Creater = creater;
            dbBugReport.IsActive = true;

            _bugReportRepository.Save(dbBugReport);

            return RedirectToAction("BugReports", "BugReport");
        }

        public IActionResult Wonderful(long bugReportId)
        {
            var bugReport = _bugReportRepository.Get(bugReportId);
            _payForActionService.EarnMoney(bugReport.Creater.Id, 10);

            return RedirectToAction("BugReports", "BugReport");
        }
    }
}