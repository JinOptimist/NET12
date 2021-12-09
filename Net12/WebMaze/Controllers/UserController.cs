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
    public class UserController : Controller
    {
        private UserRepository _userRepository;
        private UserService _userService;
        private IMapper _mapper;

        public UserController(UserRepository userRepository,
            IMapper mapper, 
            UserService userService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;
        }

        [Authorize]
        public IActionResult Profile()
        {
            var user = _userService.GetCurrentUser();
            var userViewModel = _mapper.Map<UserViewModel>(user);
            return View(userViewModel);
        }

        [Authorize]
        public IActionResult NewCoin(int coins)
        {
            var user = _userService.GetCurrentUser();
            user.Coins = coins;
            _userRepository.Save(user);
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult Login()
        {
            var viewModel = new LoginViewModel() { ReturnUrl = Request.Headers["Referer"].ToString() };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            var user = _userRepository.GetByNameAndPassword(viewModel.Login, viewModel.Password);

            var claims = new List<Claim>();

            claims.Add(new Claim("Id", user.Id.ToString()));
            claims.Add(new Claim("Name", user.Name));
            claims.Add(new Claim("Age", user.Age.ToString()));
            claims.Add(new Claim(ClaimTypes.AuthenticationMethod, Startup.AuthCoockieName));

            var claimsIdentity = new ClaimsIdentity(claims, Startup.AuthCoockieName);

            var principal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(principal);

            return Redirect(viewModel.ReturnUrl);
        }

        public async Task<IActionResult> Logout(LoginViewModel viewModel)
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
