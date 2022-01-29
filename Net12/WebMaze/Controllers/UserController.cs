using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;
using WebMaze.SignalRHubs;

namespace WebMaze.Controllers
{
    public class UserController : Controller
    {
        private UserRepository _userRepository;
        private UserService _userService;
        private GroupListRepository _groupListRepository;
        private IMapper _mapper;

        private IHubContext<ChatHub> _chatHub;


        public UserController(UserRepository userRepository,
            IMapper mapper,
            UserService userService,
            IHubContext<ChatHub> chatHub, GroupListRepository groupListRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;
            _chatHub = chatHub;
            _groupListRepository = groupListRepository;
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
            var viewModel = new LoginViewModel();
            if (string.IsNullOrEmpty(Request.Query["ReturnUrl"]))
            {
                viewModel.ReturnUrl = "/Home/Index";
            }
            else
            {
                viewModel.ReturnUrl = Request.Query["ReturnUrl"];
            }
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            var user = _userRepository.GetByNameAndPassword(viewModel.Login, viewModel.Password);

            if(user == null)
            {
                return View(viewModel);
            }

            var claims = new List<Claim>();

            claims.Add(new Claim("Id", user.Id.ToString()));
            claims.Add(new Claim("Name", user.Name));
            claims.Add(new Claim("Age", user.Age.ToString()));
            claims.Add(new Claim(ClaimTypes.AuthenticationMethod, Startup.AuthCoockieName));

            var claimsIdentity = new ClaimsIdentity(claims, Startup.AuthCoockieName);

            var principal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(principal);

            await _chatHub.Clients.All.SendAsync("Enter", user.Name);

            return Redirect(viewModel.ReturnUrl);
        }

        public async Task<IActionResult> Logout(LoginViewModel viewModel)
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddGroup()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult MyGroup(long IdGroup)
        {
            var myGroup = _groupListRepository.Get(IdGroup);
            if(myGroup.Creator.Id != _userService.GetCurrentUser().Id)
            {
                RedirectToAction("Profile", "User");
            }
            var ViewGroup = _mapper.Map<GroupListViewModel>(myGroup);

            return View(ViewGroup);
        }
        [Authorize]
        [HttpPost]
        public IActionResult MyGroup(string UserName, long Id)
        {
            var user = _userRepository.GetAll().FirstOrDefault(c => c.Name == UserName);

            if (!_userService.GetCurrentUser().Groups.Any(c => c.Id == Id))
            {
                return RedirectToAction("Profile", "User");
            }
            if (user == null)
            {
                 return MyGroup(Id);
            }

            var myGroup = _groupListRepository.Get(Id);
            var UserInGroup = new UserInGroup()
            {
                Group = myGroup,
                IsActive = true,
                User = user,
                
            };
            myGroup.Users.Add(UserInGroup);
            _groupListRepository.Save(myGroup);
            
            return MyGroup(Id);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddGroup(GroupListViewModel group)
        {
            var MyGroup = _mapper.Map<GroupList>(group);
            MyGroup.IsActive = true;
            MyGroup.Creator = _userService.GetCurrentUser();
            _groupListRepository.Save(MyGroup);

            return RedirectToAction("Profile", "User");
        }
    }
}
