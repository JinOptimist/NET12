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

        public PayForActionService (UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool Payment(long userId, int amount)
        {
            var user = _userRepository.Get(userId);
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
    }
}
