using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace WebMaze.EfStuff.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        private ReviewRepository _reviewRepository;
        private FavGamesRepository _favGamesRepository;

        private ImageRepository _imageRepository;
        private MazeLevelRepository _mazeLevelRepository;
        private CellRepository _cellRepository;
        private MazeEnemyRepository _mazeEnemyRepository;
        public UserRepository(WebContext webContext,
            ReviewRepository reviewRepository,
            ImageRepository imageRepository, MazeLevelRepository mazeLevelRepository, CellRepository cellRepository, FavGamesRepository favGamesRepository, MazeEnemyRepository mazeEnemyRepository) : base(webContext)
        {
            _reviewRepository = reviewRepository;
            _favGamesRepository = favGamesRepository;
            _imageRepository = imageRepository;
            _mazeLevelRepository = mazeLevelRepository;
            _cellRepository = cellRepository;
            _mazeEnemyRepository = mazeEnemyRepository;
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
            foreach (var maze in user.ListMazeLevels)
            {
                _cellRepository.Remove(maze.Cells);
                _mazeEnemyRepository.Remove(maze.Enemies);
            }
            _mazeLevelRepository.Remove(user.ListMazeLevels);
            _favGamesRepository.RemoveByUser(user.Id);
            _imageRepository.RemoveByUser(user.Id);
            base.Remove(user);
        }

        public List<string> GetMagicUser(string newsId)
        {
            var param = new SqlParameter("@newsID", newsId);
            return _dbSet.FromSqlRaw<User>(@$"SELECT DISTINCT U.*
FROM
	(
		SELECT N.Id as NewsId, Max(U.Coins) userCoinsCumm
		FROM 
			News N 
			LEFT JOIN NewsComments NC ON NC.NewsId = N.Id
			LEFT JOIN Users U ON NC.AuthorId = U.Id
		WHERE N.Id = @newsID
		GROUP BY N.Id
	) TempTable
	LEFT JOIN News N ON TempTable.NewsId = N.Id
	LEFT JOIN Users U ON U.Coins = TempTable.userCoinsCumm
WHERE U.Name IS NOT NULL
", param)
                .Select(x => x.Name)
                .ToList();
        }
    }
}
