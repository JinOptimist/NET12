using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
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
            var userInGroup = myGroup.Users.Where(u => !u.UserLevel.HasFlag(GroupUserLevel.None)).ToList();
            if (!userInGroup.Any(
                u => u.User.Id == _userService.GetCurrentUser().Id
                && (u.UserLevel.HasFlag(GroupUserLevel.Member) || u.UserLevel.HasFlag(GroupUserLevel.Invited))))
            {
                return RedirectToAction("Profile", "User");
            }
            var MeUser = userInGroup.FirstOrDefault(u => u.User.Id == _userService.GetCurrentUser().Id);
            if (MeUser.UserLevel == GroupUserLevel.Invited)
            {
                MeUser.UserLevel = GroupUserLevel.Member;
                _userInGroupRepository.Save(MeUser);
            }
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

        [Authorize]
        public IActionResult DeleteFromGroup(long GroupId, long UserId)
        {
            var logger = HttpContext.RequestServices.GetService(typeof(ILogger<LocalizeMidlleware>)) as ILogger<LocalizeMidlleware>;
            var MeUser = _userService.GetCurrentUser();
            var MeUserInGroup = MeUser.UsersInGroup.SingleOrDefault(u => u.Group.Id == GroupId);
            if (!(MeUserInGroup != null && (MeUserInGroup.UserLevel.HasFlag(GroupUserLevel.Admin) || (MeUserInGroup.Id == UserId && MeUserInGroup.UserLevel.HasFlag(GroupUserLevel.Member)))))
            {
                return RedirectToAction("Profile", "User");
            }
            var DeleteUser = _userInGroupRepository.Get(UserId);
            if (DeleteUser is null || DeleteUser.Group.Id != GroupId)
            {
                return MyGroup(GroupId);
            }
            DeleteUser.UserLevel = GroupUserLevel.None;
            if(DeleteUser.Group is null)
            {
                logger.LogCritical($"Delete User without Group UserId = {DeleteUser.User.Id} | GroupUserId = {DeleteUser.Id} | Delete from GroupId = {GroupId}");
            }
            _userInGroupRepository.Save(DeleteUser);


            return RedirectToAction("MyGroup", "User", new { IdGroup = GroupId });
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddInGroup(long GroupId)
        {
            if(!_userService.GetCurrentUser().UsersInGroup.Any(u => u.Group.Id == GroupId && u.UserLevel.HasFlag(GroupUserLevel.Admin)))
            {
                return RedirectToAction("Profile", "User");
            }
            var GroupModel = _groupListRepository.Get(GroupId);
            var NoGroupUsers = _userRepository.GetAll().Where(u => !u.UsersInGroup.Any(ug => ug.Group.Id == GroupId && (ug.UserLevel.HasFlag(GroupUserLevel.Member) || ug.UserLevel.HasFlag(GroupUserLevel.Invited)))).ToList();
            var NoGroupUsersViewModel = _mapper.Map<List<UserViewModel>>(NoGroupUsers);
            var usersNotFromGroup = new UsersNotFromGroupViewModel() { GroupId = GroupId, NoGroupUsers = NoGroupUsersViewModel, };
            return View(usersNotFromGroup);
        }
        [Authorize]
        [HttpGet]
        public IActionResult AddInGroupUser(long GroupId, long UserId)
        {
            var InvitedUser = _userRepository.Get(UserId);
            var MyGroup = _groupListRepository.Get(GroupId);
            if(InvitedUser is null 
                || MyGroup is null
                || !_userService
                   .GetCurrentUser()
                   .UsersInGroup
                   .Any(u => (u.Group.Id == GroupId && u.UserLevel.HasFlag(GroupUserLevel.Admin)))) 
            {
                return RedirectToAction("Profile", "User");
            }
            if(MyGroup.Users.Any(u => u.User.Id == InvitedUser.Id))
            {
                var InvUser = MyGroup.Users.Single(u => u.User.Id == InvitedUser.Id);
                if (InvUser.UserLevel.HasFlag(GroupUserLevel.Requested))
                {
                    InvUser.UserLevel = GroupUserLevel.Member;
                }
                else
                {
                    InvUser.UserLevel = GroupUserLevel.Invited;
                }
                    
            }
            else
            {
                MyGroup.Users.Add(new UserInGroup
                {
                    IsActive = true,
                    User = InvitedUser,
                    Group = MyGroup,
                    UserLevel = GroupUserLevel.Invited,
                });
            }
            _groupListRepository.Save(MyGroup);

            return RedirectToAction("MyGroup", "User", new { IdGroup = GroupId });
        }
        
    }
}
