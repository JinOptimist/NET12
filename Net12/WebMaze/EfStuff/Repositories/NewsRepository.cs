using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class NewsRepository : BaseRepository<News>
    {
        public NewsRepository(WebContext webContext) : base(webContext)
        {
        }

        public List<News> GetAllSorted()
        {
            var table = Expression.Parameter(typeof(News), "news");// news =>
            var member = Expression.Property(table, "Title"); // news.Title
            var constName = Expression.Constant("Good news"); // 'good news'
            var eq = Expression.Equal(member, constName);// news => news.Title == 'good news'

            var condition = Expression.Lambda<Func<News, bool>>(eq, table);

            return _dbSet
                .Where(condition)
                .ToList();
        }

        public News GetNewsByName(string title)
        {
            return _dbSet.SingleOrDefault(x => x.Title == title);
        }
    }
}
