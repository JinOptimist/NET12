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
 SELECT *
FROM Users
WHERE Id in
    (SELECT CreaterId
     FROM
       (SELECT CreaterId,
               COUNT(*) AS amount
        FROM
          (SELECT CreaterId
           FROM
             (SELECT CreaterId,
                     Users.IsActive
              FROM NewCellSuggestions AS N
              INNER JOIN Users ON N.CreaterId = Users.Id) AS TEMP
           WHERE IsActive = 1) AS TEMP
        GROUP BY CreaterId) AS TEMP
     WHERE amount =
         (SELECT max(amount)
          FROM
            (SELECT CreaterId,
                    COUNT(*) AS amount
             FROM
               (SELECT CreaterId
                FROM
                  (SELECT CreaterId,
                          Users.IsActive
                   FROM NewCellSuggestions AS N
                   INNER JOIN Users ON N.CreaterId = Users.Id) AS TEMP
                WHERE IsActive = 1) AS TEMP
             GROUP BY CreaterId) AS TEMP))")
                .ToList();
        }
    }
}

