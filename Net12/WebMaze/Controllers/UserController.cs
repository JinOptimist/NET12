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
        private UserInGroupRepository _userInGroupRepository;
        private IMapper _mapper;

        private IHubContext<ChatHub> _chatHub;


        public UserController(UserRepository userRepository,
            IMapper mapper,
            UserService userService,
            IHubContext<ChatHub> chatHub, GroupListRepository groupListRepository, UserInGroupRepository userInGroupRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;
            _chatHub = chatHub;
            _groupListRepository = groupListRepository;
            _userInGroupRepository = userInGroupRepository;
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

            if (user == null)
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
        [HttpPost]
        public IActionResult AddGroup(GroupListViewModel group)
        {
            var MyGroup = _mapper.Map<GroupList>(group);
            MyGroup.IsActive = true;
            MyGroup.Creator = _userService.GetCurrentUser();
            MyGroup.Users.Add(new UserInGroup()
            {
                Group = MyGroup,
                IsActive = true,
                User = MyGroup.Creator,
                UserLevel = GroupUserLevel.Admin | GroupUserLevel.Member,
            });
            _groupListRepository.Save(MyGroup);

            return RedirectToAction("Profile", "User");
        }

        [Authorize]
        [HttpGet]
        public IActionResult MyGroup(long IdGroup)
        {
            var myGroup = _groupListRepository.Get(IdGroup);
            if (myGroup is null)
            {
                return RedirectToAction("Profile", "User");
            }
            myGroup.Users = myGroup.Users.Where(u => u.UserLevel.HasFlag(GroupUserLevel.Member) || u.UserLevel.HasFlag(GroupUserLevel.Invited)).ToList();
            if (!myGroup.Users.Any(u => u.User.Id == _userService.GetCurrentUser().Id))
            {
                return RedirectToAction("Profile", "User");
            }
            var MeUser = myGroup.Users.FirstOrDefault(u => u.User.Id == _userService.GetCurrentUser().Id);
            if (MeUser.UserLevel == GroupUserLevel.Invited)
            {
                MeUser.UserLevel = GroupUserLevel.Member;
            }
            _userInGroupRepository.Save(MeUser);
            var ViewGroup = _mapper.Map<GroupListViewModel>(myGroup);

            return View(ViewGroup);
        }
        [Authorize]
        [HttpPost]
        public IActionResult MyGroup(string UserName, long Id)
        {
            var user = _userRepository.GetAll().FirstOrDefault(c => c.Name == UserName);
            var myGroup = _groupListRepository.Get(Id);
            //if (!_userService.GetCurrentUser().Groups.Any(c => c.Id == Id))
            var MeUser = _userService.GetCurrentUser();
            if (!myGroup.Users.Any(u => u.User.Id == MeUser.Id && u.UserLevel.HasFlag(GroupUserLevel.Admin)))
            {
                return RedirectToAction("Profile", "User");
            }
            if (user == null)
            {
                return MyGroup(Id);
            }

            if (myGroup.Users.Any(u => u.User.Id == user.Id))
            {
                var IsUser = myGroup.Users.Single(u => u.User.Id == user.Id);
                if (!IsUser.UserLevel.HasFlag(GroupUserLevel.None))
                {
                    IsUser.UserLevel = GroupUserLevel.None;
                    _userInGroupRepository.Save(IsUser);
                }
                else
                {
                    if (IsUser.UserLevel.HasFlag(GroupUserLevel.Requested))
                    {
                        IsUser.UserLevel = GroupUserLevel.Member;
                    }
                    else
                    {
                        IsUser.UserLevel = GroupUserLevel.Invited;
                    }

                    _userInGroupRepository.Save(IsUser);
                }

                return MyGroup(Id);
            }
            else
            {
                var UserInGroup = new UserInGroup()
                {
                    Group = myGroup,
                    IsActive = true,
                    User = user,
                    UserLevel = GroupUserLevel.Invited,

                };
                //myGroup.Users.Add(UserInGroup);
                _userInGroupRepository.Save(UserInGroup);

                return MyGroup(Id);
            }

        }

    }
}
