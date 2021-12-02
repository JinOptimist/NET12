using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
