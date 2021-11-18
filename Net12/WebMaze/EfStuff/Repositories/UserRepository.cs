using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class UserRepository
    {
        private WebContext _webContext;

        public UserRepository(WebContext webContext)
        {
            _webContext = webContext;
        }

        public User Get(long id)
        {
            return _webContext.Users.SingleOrDefault(x => x.Id == id);
        }

        public List<User> GetAll()
        {
            return _webContext
                .Users
                .Where(x => x.IsActive)
                .ToList();
        }

        public User GetRandomUser()
        {
            return _webContext.Users.First();
        }

        public void Save(User user)
        {
            if (user.Id > 0)
            {
                _webContext.Update(user);
            }
            else
            {
                _webContext.Users.Add(user);
            }
            
            _webContext.SaveChanges();
        }

        public void Remove(User user)
        {
            user.IsActive = false;
            new ReviewRepository(_webContext).Remove(user.MyReviews);
            Save(user);
        }

        public void Remove(long userId)
        {
            var user = Get(userId);
            Remove(user);
        }
    }
}
