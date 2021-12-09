using Microsoft.EntityFrameworkCore;
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
        private MazeLevelRepository _mazeLevelRepository;
        private CellRepository _cellRepository;
        public UserRepository(WebContext webContext,
            ReviewRepository reviewRepository,
            ImageRepository imageRepository, MazeLevelRepository mazeLevelRepository, CellRepository cellRepository) : base(webContext)
        {
            _reviewRepository = reviewRepository;
            _imageRepository = imageRepository;
            _mazeLevelRepository = mazeLevelRepository;
            _cellRepository = cellRepository;
        }

        public User GetByNameAndPassword(string login, string password)
        {
            return _dbSet.SingleOrDefault(x => x.Name == login && x.Password == password);
        }

        public User GetRandomUser()
        {
            return _webContext.Users.First();
        }

        public User GetUserByName(string name)
        {
            return _dbSet.SingleOrDefault(x => x.Name == name);
        }

        public override void Remove(User user)
        {
            _reviewRepository.Remove(user.MyReviews);
            foreach(var maze in user.ListMazeLevels)
            {
                _cellRepository.Remove(maze.Cells);
            }
            _mazeLevelRepository.Remove(user.ListMazeLevels);
            _imageRepository.RemoveByUser(user.Id);
            base.Remove(user);
        }
    }
}
