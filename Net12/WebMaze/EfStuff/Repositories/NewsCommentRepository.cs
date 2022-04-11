using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class NewsCommentRepository : BaseRepository<NewsComment>
    {
        public NewsCommentRepository(WebContext webContext) : base(webContext)
        {
        }

        public NewsComment GetNewsCommentByText(string text)
        {
            return _dbSet.SingleOrDefault(x => x.Text == text);
        }

        public List<NewsComment> GetAllId(long id)
        {
            return _dbSet.Where(x => x.News.Id == id && x.IsActive).ToList();
        }

        public List<NewsComment>GetAllUser(long userId)
        {
            return _dbSet.Where(x => x.Author.Id == userId && x.IsActive).ToList();
        }
    }
}