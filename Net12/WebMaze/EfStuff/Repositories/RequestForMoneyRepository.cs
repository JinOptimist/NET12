using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.Services;

namespace WebMaze.EfStuff.Repositories
{
    public class RequestForMoneyRepository : BaseRepository<RequestForMoney>
    {
        private UserRepository _userRepository;
        private RequestForMoneyRepository _requestForMoneyRepository;

        public RequestForMoneyRepository(WebContext webContext, UserRepository userRepository,
            RequestForMoneyRepository requestForMoneyRepository) : base(webContext)
        {
            _userRepository = userRepository;
            _requestForMoneyRepository = requestForMoneyRepository;
        }
        
        public virtual List<RequestForMoney> GetCurrentUserRequests(long userId)
        {
            return _dbSet.Where(g =>
                    g.RequestStatus == Services.RequestStatusEnums.WaitingForAnAnswer
            &&
            g.RequestRecipient.Id == userId).ToList();
        }
        public virtual List<RequestForMoney> GetRequestsToTheCurrentUser(long userId)
        {
            return _dbSet.Where(g =>
                    g.RequestStatus == Services.RequestStatusEnums.WaitingForAnAnswer
            &&
            g.RequestCreator.Id == userId).ToList();
        }
        public virtual List<RequestForMoney> GetAllRequestsCurrentUser(long userId)
        {
            return _dbSet.Where(g =>
                    g.RequestRecipient.Id == userId
           ||
            g.RequestCreator.Id == userId).ToList();
        }

        public bool TrasactionRequest(RequestForMoney request, User requestCreator, User requestRecipient)
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
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }

            return true;
        }


    }
}
