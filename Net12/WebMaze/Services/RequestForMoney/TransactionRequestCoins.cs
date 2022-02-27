using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;

namespace WebMaze.Services.RequestForMoney
{
    public class TransactionRequestCoins
    {
        private UserRepository _userRepository;
        private RequestForMoneyRepository _requestForMoneyRepository;
        private WebContext _webContext;

        public TransactionRequestCoins(UserRepository userRepository,
            RequestForMoneyRepository requestForMoneyRepository, WebContext webContext)
        {
            _userRepository = userRepository;
            _requestForMoneyRepository = requestForMoneyRepository;
            _webContext = webContext ?? throw new ArgumentException(nameof(webContext));
        }


        public bool AttemptTransactionRequest(EfStuff.DbModel.RequestForMoney request, User requestCreator, User requestRecipient)
        {
            using (var transaction = _webContext.Database.BeginTransaction())
            {

                try
                {
                    requestCreator.Coins = requestCreator.Coins + request.RequestAmount;
                    _userRepository.Save(requestCreator);

                    requestRecipient.Coins = requestRecipient.Coins - request.RequestAmount;
                    _userRepository.Save(requestRecipient);

                    request.RequestStatus = RequestStatusEnums.RequestApproved;
                    _requestForMoneyRepository.Save(request);


                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }

        }
    }
}
