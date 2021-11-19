using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class NewsRepository
    {
        private WebContext _webContext;

        public NewsRepository(WebContext webContext)
        {
            _webContext = webContext;
        }

        public News Get(long id)
        {
            return _webContext.News.SingleOrDefault(x => x.Id == id);
        }

        public List<News> GetAll()
        {
            return _webContext
                .News
                .Where(x => x.IsActive)
                .ToList();
        }

        public News GetRandomUser()
        {
            return _webContext.News.First();
        }

        public void Save(News news)
        {
            if (news.Id > 0)
            {
                _webContext.Update(news);
            }
            else
            {
                _webContext.News.Add(news);
            }
            
            _webContext.SaveChanges();
        }

        public void Remove(News news)
        {
            news.IsActive = false;
            Save(news);
        }

        public void Remove(long newsId)
        {
            var news = Get(newsId);
            Remove(news);
        }
    }
}
