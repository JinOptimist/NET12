using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models.Enums;

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

        public bool Payment(TypesOfPayment typeOfPayment)
        {
            var user = _userService.GetCurrentUser();
            if (user.Coins < (int)typeOfPayment)
            {
                return false;
            }

            user.Coins -= (int)typeOfPayment;
            _userRepository.Save(user);

            return true;
        }

        public void CreatorEarnMoney(long userId, int amount)
        {
            var user = _userRepository.Get(userId);

            user.Coins += amount;
            _userRepository.Save(user);
        }

        public void CreatorDislikeFine(long userId, TypesOfPayment typesOfPayment)
        {
            var user = _userRepository.Get(userId);

            if(user.Coins < (int)typesOfPayment)
            {                
                user.Coins -= user.Coins;
                _userRepository.Save(user);
            } 
            else
            {
                Payment(typesOfPayment);
            }
        }
    }
}
