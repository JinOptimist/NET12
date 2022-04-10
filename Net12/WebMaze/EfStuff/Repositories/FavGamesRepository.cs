using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public List<Game> GetFavGamesSortedByProperty(string gameFilter, bool ascDirection)
        {
            var table = Expression.Parameter(typeof(Game), "game");

            gameFilter ??= "Name";

            var members = new List<MemberExpression>();
            var newWords = gameFilter.Split('.');

            members.Add(Expression.Property(table, newWords[0]));
            var counter = 1;

            while (counter < newWords.Length)
            {
                members.Add(Expression.Property(members[counter - 1], newWords[counter]));
                counter++;
            };

            var condition = Expression.Lambda<Func<Game, object>>(Expression.Convert(members.Last(), typeof(object)), table);

            return (ascDirection ? _dbSet.OrderBy(condition) : _dbSet.OrderByDescending(condition))
                    .ToList();
        }

    }
}
