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
SELECT U.*
FROM
  (SELECT CreaterId AS cId
   FROM
     (SELECT CreaterId,
             COUNT(*) AS amount
      FROM NewCellSuggestions AS N
      INNER JOIN Users ON N.CreaterId = Users.Id
      WHERE users.isactive = 1
      GROUP BY CreaterId) AS TEMP
   WHERE amount =
       (SELECT max(amount)
        FROM
          (SELECT CreaterId,
                  COUNT(*) AS amount
           FROM NewCellSuggestions AS N
           INNER JOIN Users ON N.CreaterId = Users.Id
           WHERE users.isactive = 1
           GROUP BY CreaterId) AS TEMP)) AS GoodIds
LEFT JOIN Users U ON U.Id = GoodIds.cId")
                .ToList();
        }
    }
}

