using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }
    }
}
