using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class ReviewRepository
    {
        private WebContext _webContext;

        public ReviewRepository(WebContext webContext)
        {
            _webContext = webContext;
        }

        public List<Review> GetAll()
        {
            return _webContext.Reviews.Where(act => act.IsActive == true).ToList();
        }
        public Review Get(long id)
        {
            return _webContext.Reviews.SingleOrDefault(rev => rev.Id == id);
        }


        public void Save(Review review)
        {
            if (review.Id > 0)
            {
                _webContext.Update(review);
            }
            else
            {
                _webContext.Reviews.Add(review);
            }
            _webContext.SaveChanges();
        }
        public void Remove(long id)
        {
            Remove(Get(id));
        }
        public void Remove(Review review)
        {
            review.IsActive = false;
            _webContext.Reviews.Update(review);
            _webContext.SaveChanges();
        }
        public void Remove(List<Review> reviews)
        {
            foreach (Review review in reviews)
            {
                review.IsActive = false;
                _webContext.Reviews.Update(review);
            }
            _webContext.SaveChanges();

        }
    }
}
