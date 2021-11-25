using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;

namespace WebMaze.Services
{
    public class UserService
    {
        private UserRepository _userRepository;

        private IHttpContextAccessor _httpContextAccessor;

        public UserService(UserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public User GetCurrentUser()
        {
            var claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id");
            var idStr = claim.Value;
            var id = int.Parse(idStr);

            var user = _userRepository.Get(id);
            return user;
        }
    }
}
