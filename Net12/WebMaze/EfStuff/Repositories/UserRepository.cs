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
        private ImageRepository _imageRepository;

        public UserRepository(WebContext webContext, 
            ReviewRepository reviewRepository,
            ImageRepository imageRepository) : base(webContext)
        {
            _reviewRepository = reviewRepository;
            _imageRepository = imageRepository;
        }

        public User GetByNameAndPassword(string login, string password)
        {
            return _dbSet.SingleOrDefault(x => x.Name == login && x.Password == password);
        }

        public User GetRandomUser()
        {
            return _webContext.Users.First();
        }

        public override void Remove(User user)
        {
            _reviewRepository.Remove(user.MyReviews);
            _imageRepository.RemoveByUser(user.Id);
            base.Remove(user);
        }
    }
}
