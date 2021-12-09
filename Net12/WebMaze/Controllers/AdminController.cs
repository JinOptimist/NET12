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

        private IMapper _mapper;

        public AdminController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [Authorize]
        public IActionResult ViewPermission()
        {
            return View();
        }




    }
}
