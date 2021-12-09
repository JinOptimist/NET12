using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.Repositories;

namespace WebMaze.Services
{
    public class PayForActionService
    {
        readonly private UserRepository _userRepository;
        readonly private UserService _userService;

        public PayForActionService (UserRepository userRepository, UserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        /*public bool Payment(long userId, int amount)
        {
            var user = _userRepository.Get(userId);
            if (user.Coins < amount)
            {
                return false;
            }

            user.Coins -= amount;
            _userRepository.Save(user);

            return true;
        }*/

        public bool Payment(int amount)
        {
            var user = _userService.GetCurrentUser();
            if (user.Coins < amount)
            {
                return false;
            }

            user.Coins -= amount;
            _userRepository.Save(user);

            return true;
        }

        public void EarnMoney(long userId, int amount)
        {
            var user = _userRepository.Get(userId);

            user.Coins += amount;
            _userRepository.Save(user);
        }

        public void EarnMoney(string userName, int amount)
        {
            var user = _userRepository.GetUserByName(userName); ;

            user.Coins += amount;
            _userRepository.Save(user);
        }
    }
}
