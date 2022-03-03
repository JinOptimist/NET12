using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models.Enums;

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
            if (claim == null)
            {
                return null;
            }
            var idStr = claim.Value;
            var id = int.Parse(idStr);

            var user = _userRepository.Get(id);
            return user;
        }

        public bool IsAdmin
            => GetCurrentUser()?.Perrmissions.Any(x => x.Name == Perrmission.Admin) ?? false;

        public void AddCoins(int coins)
        {

            var user = GetCurrentUser();
            user.Coins += coins;
            _userRepository.Save(user);
        }

        public bool RemoveCoins(int coins)
        {
            var user = GetCurrentUser();

            if (user.Coins - coins < 0)
            {
                return false;
            }
            else
            {
                user.Coins -= coins;
                _userRepository.Save(user);
                return true;
            }
        }

        public List<string> AnswerInfoMessage(InfoMessages infoMessage)
        {
            var answer = new List<string>
            {
                ResourceLocalization.InfoMessageLocalize.ResourceManager.GetString(infoMessage.ToString())
            };

            if ((int)infoMessage < 1000)
            {
                answer.Add("green");
            }
            else
            {
                answer.Add("red");
            }
            return answer;
        }

    }
}
