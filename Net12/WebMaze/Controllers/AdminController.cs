using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net12.Maze;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using WebMaze.Controllers.AuthAttribute;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    [IsAdmin]
    public class AdminController : Controller
    {
        private PermissionRepository _permissionRepository;
        private IMapper _mapper;
        private UserRepository _userRepository;
        public static bool notReapitUsersName = false;

        public AdminController(PermissionRepository permissionRepository, IMapper mapper,
            UserRepository userRepository)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public IActionResult ViewPermission()
        {
            var permissionViewModels = new List<PermissionViewModel>();
            permissionViewModels = _permissionRepository.GetAll()
                .Select(x => _mapper.Map<PermissionViewModel>(x)).ToList();
            return View(permissionViewModels);
        }


        public IActionResult EditingUsers()
        {
            return View();
        }

        public IActionResult ReflectionPages()
        {
            var controllers = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.BaseType == typeof(Controller));

            var viewModels = new List<ControllerViewModel>();

            foreach (var controller in controllers)
            {
                var viewModel = new ControllerViewModel();

                viewModel.Name = controller.Name.Replace("Controller", "");

                var actions = controller
                    .GetMethods()
                    .Where(x => x.ReturnType == typeof(IActionResult));

                viewModel.Actions = new List<ActionViewModel>();
                foreach (var action in actions)
                {
                    var actionViewModel = new ActionViewModel();

                    actionViewModel.Name = action.Name;
                    actionViewModel.AttributeNames = action
                        .CustomAttributes
                        .Select(x => x.AttributeType.Name.Replace("Attribute", ""))
                        .ToList();

                    actionViewModel.ParamsNames = action
                        .GetParameters()
                        .Select(x => x.Name)
                        .ToList();

                    viewModel.Actions.Add(actionViewModel);
                }

                viewModel.ActionCount = actions.Count();

                viewModel.AttributeNames = controller
                    .CustomAttributes
                    .Select(x => x.AttributeType.Name.Replace("Attribute", ""))
                    .ToList();

                viewModels.Add(viewModel);
            }

            return View(viewModels);
        }
        public IActionResult CellInfoHelper()
        {
            var typeOfCell = new List<Type>() { typeof(BaseCell) };
            typeOfCell.AddRange(TypeCollector(typeOfCell));

            var namesTypeOfCell = typeOfCell
                .Select(x => x.Name)
                .Select(x => x.ToLower())
                .ToList();

            //section on removing base and intermediate types
            var noShowTypes = new List<string>() { "BaseCell", "BaseEnemy", "Character" }
            .Select(x => x.ToLower())
            .ToList();
            namesTypeOfCell.RemoveAll(x => noShowTypes.Contains(x));

            var namesOfActions = typeof(CellInfoController)
               .GetMethods()
               .Where(x => x.ReturnType == typeof(IActionResult))
               .Select(x => x.Name)
               .Select(x => x.ToLower())
               .ToList();

            namesTypeOfCell.RemoveAll(x => namesOfActions.Contains(x));

            return View(namesTypeOfCell);
        }

        public List<Type> TypeCollector(List<Type> inTypes)
        {
            List<Type> outTypes = new List<Type>();
            foreach (var item in inTypes)
            {
                var heirs = Assembly.GetAssembly(typeof(BaseCell))
                .GetTypes()
                .Where(x => x.BaseType == item)
                .ToList();

                outTypes.AddRange(heirs);
            }

            if (outTypes.Any())
            {
                outTypes.AddRange(TypeCollector(outTypes));
            }
            return outTypes;
        }

        public IActionResult DownloadTreeControllers()
        {
            using (var ms = new MemoryStream())
            {
                using (var wordDocument = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document))
                {
                    var controllers = Assembly
                        .GetExecutingAssembly()
                        .GetTypes()
                        .Where(x => x.BaseType == typeof(Controller));

                    var mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = mainPart.Document.AppendChild(new Body());

                    foreach (var controller in controllers)
                    {

                        var para = body.AppendChild(new Paragraph());
                        var runTitle = para.AppendChild(new Run());
                        var prop = runTitle.AppendChild(new RunProperties());

                        prop.AppendChild(new Color { Val = "008080" });
                        prop.AppendChild(new FontSize { Val = "36" });

                        var text = runTitle.AppendChild(new Text());
                        text.Text = controller.Name + "()";

                        var actions = controller
                            .GetMethods()
                            .Where(x => x.ReturnType == typeof(IActionResult));

                        foreach (var action in actions)
                        {
                            var paramsString = "";

                            foreach (var param in action.GetParameters())
                            {
                                paramsString += param.Name + ", ";
                            }

                            if (paramsString.Length > 0)
                            {
                                paramsString = paramsString.Remove(paramsString.Length - 2);
                            }

                            var runTitleAction = para.AppendChild(new Run());
                            runTitleAction.AppendChild(new RunProperties { Color = new Color { Val = "006400" } });
                            runTitleAction.AppendChild(new Break());
                            runTitleAction.AppendChild(new TabChar());
                            runTitleAction.AppendChild(new Text(action.Name.ToString() + "("));

                            var runTitleAction2 = para.AppendChild(new Run());
                            runTitleAction2.AppendChild(new RunProperties { Color = new Color { Val = "191970" } });
                            runTitleAction2.AppendChild(new Text(paramsString));

                            var runTitleAction3 = para.AppendChild(new Run());
                            runTitleAction3.AppendChild(new RunProperties { Color = new Color { Val = "006400" } });
                            runTitleAction3.AppendChild(new Text(")"));
                        }
                    }

                    wordDocument.Close();
                }

                return File(ms.ToArray(),
                   "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                   "ReflectionPages.docx");
            }
        }

        public IActionResult DownlodListReapitUsers()
        {
            using (var ms = new MemoryStream())
            {
                using (var wordDocument = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document))
                {
                    var listUsers = _userRepository.GetReapitUsersNameSQL();
                    if (listUsers.Any())
                    {
                        var mainPart = wordDocument.AddMainDocumentPart();
                        mainPart.Document = new Document();
                        var body = mainPart.Document.AppendChild(new Body());

                        foreach (var user in listUsers)
                        {
                            var para = body.AppendChild(new Paragraph());
                            var runTitle = para.AppendChild(new Run());
                            runTitle.AppendChild(new Text($"{user.Name} - {user.Id}"));
                            var properties = new ParagraphProperties();
                            var fontSize = new FontSize() { Val = "36" };
                            properties.Append(fontSize);
                            para.Append(properties);
                        }

                        wordDocument.Close();

                        return File(ms.ToArray(),
                           "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                           "ReapitUsersName.docx");

                    }
                    notReapitUsersName = true;
                    return RedirectToAction("Profile", "User");
                }
            }
        }
    }
}
