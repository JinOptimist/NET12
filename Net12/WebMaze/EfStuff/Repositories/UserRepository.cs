using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebMaze.EfStuff.DbModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using WebMaze.Services;
using WebMaze.Services.RequestForMoney;

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
        private ILogger<UserRepository> _logger;

        private Random _random = new Random();

        public UserRepository(WebContext webContext,
            ReviewRepository reviewRepository,
            ImageRepository imageRepository, MazeLevelRepository mazeLevelRepository,
            CellRepository cellRepository, FavGamesRepository favGamesRepository,
            MazeEnemyRepository mazeEnemyRepository,
            ILogger<UserRepository> logger) : base(webContext)
        {
            _reviewRepository = reviewRepository;
            _favGamesRepository = favGamesRepository;
            _imageRepository = imageRepository;
            _mazeLevelRepository = mazeLevelRepository;
            _cellRepository = cellRepository;
            _mazeEnemyRepository = mazeEnemyRepository;
            _logger = logger;
        }

        public User GetByNameAndPassword(string login, string password)
        {
            return GetAllQueryable().SingleOrDefault(x => x.Name == login && x.Password == password);
        }

        public User GetRandomUser()
        {
            return GetAllQueryable().First();
        }

        public User GetFullRandomUser() => GetAllQueryable().Skip(_random.Next(Count())).First();

        public User GetUserByName(string name)
        {
            return GetAllQueryable().FirstOrDefault(x => x.Name == name);
        }

        public User GetUserById(long Id)
        {
            return GetAllQueryable().SingleOrDefault(x => x.Id == Id);
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
        public List<User> GetReapitUsersNameSQL()
        {
            return _dbSet.FromSqlRaw<User>($@"
SELECT U.*
FROM   users U
       INNER JOIN (SELECT id,
                          Min (NAME) NAME
                   FROM   (SELECT TempU.NAME,
                                  TempU.id
                           FROM   (SELECT U.NAME,
                                          U.id
                                   FROM   users U
                                   WHERE  U.isactive = 1) TempU
                                  LEFT JOIN users U
                                         ON TempU.id != U.id
                           WHERE  TempU.NAME = U.NAME) TempAll
                   WHERE  TempAll.id NOT IN (SELECT RepeatUser.id
                                             FROM
                          (SELECT TempU.NAME,
                                  Min(TempU.id) Id
                           FROM   (SELECT U.NAME,
                                          U.id
                                   FROM   users U)
                                  TempU
                                            LEFT JOIN users U
                                                   ON TempU.id != U.id
                                                     WHERE  TempU.NAME = U.NAME
                                                     GROUP  BY TempU.NAME)
                          RepeatUser
                                            )
                   GROUP  BY id) R
               ON U.id = R.id 
    ")
                .ToList();
        }

        public List<User> GetReapitUsersName()
        {
            return _webContext.Users
                .ToList()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Id)
                .GroupBy(x => x.Name)
                .SelectMany(gr => gr.Skip(1))
                .ToList();
        }

        public bool TransactionCoins(long currUserId, long destUserId, int coins)
        {
            using (var transaction = _webContext.Database.BeginTransaction())
            {
                try
                {
                    var currUserIdParam = new SqlParameter("@currUserId", currUserId);
                    var destUserIdParam = new SqlParameter("@destUserId", destUserId);
                    var coinsParam = new SqlParameter("@coins", coins);

                    _webContext.Database.ExecuteSqlRaw(@"update Users
                                                        set Coins -= @coins
                                                        where Id = @currUserId", currUserIdParam, coinsParam);

                    _webContext.Database.ExecuteSqlRaw(@"update Users
                                                        set Coins += @coins
                                                        where Id = @destUserId", destUserIdParam, coinsParam);

                    _webContext.SaveChanges();
                    transaction.Commit();

                    _logger.LogInformation($"Transaction between Ids {currUserId} and {destUserId} to transfer {coins} coins was successful ");
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    _logger.LogError($"Transaction between Ids {currUserId} and {destUserId} to transfer {coins} coins was FAIL ");
                    return false;
                }
            }
            return true;
        }
    }
}
