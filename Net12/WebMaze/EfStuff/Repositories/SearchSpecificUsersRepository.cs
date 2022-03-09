using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class SearchSpecificUsersRepository : BaseRepository<User>
    {
        public SearchSpecificUsersRepository(WebContext webContext) : base(webContext)
        {
        }

        public List<User> GetWonUsersInNewCellSugg()
        {
            return _dbSet.FromSqlRaw<User>($@"
 SELECT * FROM Users WHERE Id in
(SELECT CreaterId FROM  (SELECT CreaterId, COUNT(*) as amount FROM (SELECT CreaterId FROM
(SELECT CreaterId, Users.IsActive FROM NewCellSuggestions as N INNER JOIN Users ON N.CreaterId = Users.Id) as temp WHERE IsActive = 1) as temp GROUP BY CreaterId) as temp
WHERE amount = (SELECT max(amount) from (SELECT CreaterId, COUNT(*) as amount FROM (SELECT CreaterId FROM
(SELECT CreaterId, Users.IsActive FROM NewCellSuggestions as N INNER JOIN Users ON N.CreaterId = Users.Id) as temp WHERE IsActive = 1) as temp GROUP BY CreaterId) as temp))  
    ")
                .ToList();
        }
    }
}

