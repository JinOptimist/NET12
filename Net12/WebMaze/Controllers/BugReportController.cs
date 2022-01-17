using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Net12.Maze;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Controllers.AuthAttribute;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Models.Enums;
using WebMaze.Services;
using WebMaze.SignalRHubs;

namespace WebMaze.Controllers
{
    public class BugReportController : Controller
    {
        private UserRepository _userRepository;
        private BugReportRepository _bugReportRepository;
        private IMapper _mapper;
        private UserService _userService;
        private readonly PayForActionService _payForActionService;

        private IHubContext<ChatHub> _chatHub;

        public BugReportController(UserRepository userRepository,
            BugReportRepository bugReportRepository,
            IMapper mapper,
            UserService userService,
            IHubContext<ChatHub> chatHub,
            PayForActionService payForActionService)
        {
            _userRepository = userRepository;
            _bugReportRepository = bugReportRepository;
            _mapper = mapper;
            _userService = userService;
            _payForActionService = payForActionService;
            _chatHub = chatHub;
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
        [PayForAddActionFilter(TypesOfPayment.Medium)]
        [HttpPost]
        public IActionResult AddBugReport(BugReportViewModel bugReportViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(bugReportViewModel);
            }

            var creater = _userService.GetCurrentUser();

            var dbBugReport = _mapper.Map<BugReport>(bugReportViewModel);
            dbBugReport.Creater = creater;
            dbBugReport.IsActive = true;

            _bugReportRepository.Save(dbBugReport);

            _chatHub.Clients.All.SendAsync("NewBugReport", creater.Name);

            return RedirectToAction("BugReports", "BugReport");
        }

        public IActionResult Wonderful(long bugReportId)
        {
            var bugReport = _bugReportRepository.Get(bugReportId);
            _payForActionService.CreatorEarnMoney(bugReport.Creater.Id, 10);

            return RedirectToAction("BugReports", "BugReport");
        }

        public IActionResult DownloadAll()
        {
            var reports = _bugReportRepository.GetAll();
            using (var ms = new MemoryStream())
            {
                using (var wordDocument = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document))
                {
                    var mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = mainPart.Document.AppendChild(new Body());

                    foreach (var oneReport in reports)
                    {
                        var paraCreater = body.AppendChild(new Paragraph());

                        var runCreater = paraCreater.AppendChild(new Run());
                        runCreater.AppendChild(new Text(oneReport.Creater.Name));

                        var paraDescription = body.AppendChild(new Paragraph());
                        var runDescription = paraDescription.AppendChild(new Run());
                        runDescription.AppendChild(new Text(oneReport.Description));
                    }

                    wordDocument.Close();
                }

                return File(ms.ToArray(),
                   "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                   "AllReports.docx");
            }
        }
    }
}