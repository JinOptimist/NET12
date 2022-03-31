using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Services.RequestForMoney
{
    public enum MassegeErrorsRequestEnums
    {   
        NoMassege =0,
        NotEnoughUser = 1,
        UserNotEnoughCoins = 2,
        NegativeValue = 3,       
        RequestToYourself = 4,
        NotEnoughCoins = 5
    }
}
