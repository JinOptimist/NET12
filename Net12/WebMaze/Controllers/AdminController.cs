using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net12.Maze;
using System;
using System.Collections.Generic;
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

        public AdminController(PermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
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
            var typeOfCell = new List<Type>() { (typeof(BaseCell)) };
            typeOfCell.AddRange(TypeCollector(typeOfCell));

            var namesTypeOfCell = typeOfCell
                .Select(x => x.Name)
                .Select(x => x.ToLower())
                .ToList();

            //section on removing base and intermediate types
            var removeTypes = new List<string>() { "BaseCell", "BaseEnemy", "Character" };
            removeTypes = removeTypes.Select(x => x.ToLower()).ToList();
            namesTypeOfCell.RemoveAll(x => removeTypes.Contains(x));

            var namesOfActions = Assembly
               .GetExecutingAssembly()
               .GetTypes()
               .SingleOrDefault(x => x == typeof(CellInfoController))
               .GetMethods()
               .Where(x => x.ReturnType == typeof(IActionResult))
               .Select(x => x.Name)
               .Select(x => x.ToLower())
               .ToList();

            namesTypeOfCell.RemoveAll(x => namesOfActions.Contains(x));

            return View(namesTypeOfCell);
        }

        public static List<Type> TypeCollector(List<Type> inList)
        {
            List<Type> A = new List<Type>();
            foreach (var item in inList)
            {
                var heirs = Assembly.GetAssembly(typeof(BaseCell))
                .GetTypes()
                .Where(x => x.BaseType == item)
                .ToList();

                A.AddRange(heirs);
            }

            if (A.Count != 0)
            {
                A.AddRange(TypeCollector(A));
            }
            return A;
        }

    }
}
