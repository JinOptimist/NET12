using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class AdminController : Controller
    {
        private PermissionRepository _permissionRepository;
        private IMapper _mapper;

        public AdminController(PermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        [Authorize]
        public IActionResult ViewPermission()
        {
            var permissionViewModels = new List<PermissionViewModel>();
            permissionViewModels = _permissionRepository.GetAll()
                .Select(x => _mapper.Map<PermissionViewModel>(x)).ToList();
            return View(permissionViewModels);
        }




    }
}
