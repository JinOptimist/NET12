using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class RequestForMoneyRepository : BaseRepository<RequestForMoney>
    {
        public RequestForMoneyRepository(WebContext webContext) : base(webContext)
        {

        }
        //public virtual RequestForMoney GetCurrentRequest(long userId)
        //{
        //    return (RequestForMoney) _dbSet.Where(g => g.RequestRecipient.Id == userId);
        //}
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

        //public bool TrasactionRequest(long requestId)
        //{

        //    using (var transaction = _webContext.Database.BeginTransaction())
        //    {
        //        var requestRecipient = _userService.GetCurrentUser();
        //        var request = _requestForMoneyRepository.Get(requestId);
        //        var requestCreator = _userRepository.Get(request.RequestCreator.Id);
        //        try
        //        {
        //            requestCreator.Coins = requestCreator.Coins + request.RequestAmount;
        //            _userRepository.Save(requestCreator);

        //            requestRecipient.Coins = requestRecipient.Coins - request.RequestAmount;
        //            _userRepository.Save(requestRecipient);

        //            request.RequestStatus = RequestStatusEnums.RequestApproved;
        //            _requestForMoneyRepository.Save(request);

        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //        }
        //    }
        //}


    }
}
