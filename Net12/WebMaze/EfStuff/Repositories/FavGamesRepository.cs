using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class FavGamesRepository : BaseRepository<Game>
    {
        public FavGamesRepository(WebContext webContext) : base(webContext)
        {
        }
        public void RemoveByUser(long userId)
        {
            var targetGames = _dbSet.Where(x => x.Creater != null && x.Creater.Id == userId).ToList();
            targetGames.ForEach(x => Remove(x));
        }

        public Game GetFavGameByDesc(string desc)
        {
            return _dbSet.SingleOrDefault(x => x.Desc == desc);
        }

    }
}
