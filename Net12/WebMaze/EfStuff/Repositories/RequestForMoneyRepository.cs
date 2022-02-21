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


        public RequestForMoneyRepository(WebContext webContext) : base(webContext)
        {

        }

        public virtual List<RequestForMoney> GetCurrentUserRequests(long userId)
        {
            return _dbSet.Where(g =>
                    g.RequestStatus == Services.RequestStatusEnums.WaitingForAnAnswer &&
                    g.RequestRecipient.Id == userId).ToList();
        }
        public virtual List<RequestForMoney> GetRequestsToTheCurrentUser(long userId)
        {
            return _dbSet.Where(g =>
                    g.RequestStatus == Services.RequestStatusEnums.WaitingForAnAnswer &&
                    g.RequestCreator.Id == userId).ToList();
        }
        public virtual List<RequestForMoney> GetAllRequestsCurrentUser(long userId)
        {
            return _dbSet.Where(g =>
                    g.RequestRecipient.Id == userId ||
                    g.RequestCreator.Id == userId).ToList();
        }

        public bool AttemptTrasactionRequest(RequestForMoney request, User requestCreator, User requestRecipient,
            RequestForMoneyRepository _requestForMoneyRepository, UserRepository _userRepository)
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
