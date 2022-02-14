using System;
using System.Collections.Generic;
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

        public News GetNewsByName(string title)
        {
            return _dbSet.SingleOrDefault(x => x.Title == title);
        }
        

        public List<News> GetForPagination(int perPage, int page, string columnName = "CreationDate")
            => GetSortedNews(columnName)
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToList();
    }
}
