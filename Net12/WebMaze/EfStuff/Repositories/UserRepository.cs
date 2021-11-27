using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        private ReviewRepository _reviewRepository;
        private FavGamesRepository _favGamesRepository;

        public UserRepository(WebContext webContext, 
            ReviewRepository reviewRepository, FavGamesRepository favGamesRepository) : base(webContext)
        {
            _reviewRepository = reviewRepository;
            _favGamesRepository = favGamesRepository;
        }

        public User GetRandomUser()
        {
            return _webContext.Users.First();
        }

        public override void Remove(User user)
        {
            _reviewRepository.Remove(user.MyReviews);
            _favGamesRepository.RemoveByUser(user.Id);
            base.Remove(user);
        }
    }
}
