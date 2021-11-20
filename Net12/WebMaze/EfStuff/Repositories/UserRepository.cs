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

        public UserRepository(WebContext webContext, 
            ReviewRepository reviewRepository) : base(webContext)
        {
            _reviewRepository = reviewRepository;
        }

        public User GetRandomUser()
        {
            return _webContext.Users.First();
        }

        public override void Remove(User user)
        {
            _reviewRepository.Remove(user.MyReviews);
            base.Remove(user);
        }
    }
}
