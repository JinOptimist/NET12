using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class NewCellSuggRepository
    {
        private WebContext _webContext;

        public NewCellSuggRepository(WebContext webContext)
        {
            _webContext = webContext;
        }

        public NewCellSuggestion Get(long id)
        {
            return _webContext.NewCellSuggestions.SingleOrDefault(x => x.Id == id);
        }

        public List<NewCellSuggestion> GetAll()
        {
            return _webContext
            .NewCellSuggestions
            .ToList();
        }

        public NewCellSuggestion GetRandomUser()
        {
            return _webContext.NewCellSuggestions.First();
        }

        public void Save(NewCellSuggestion user)
        {
            if (user.Id > 0)
            {
                _webContext.Update(user);
            }
            else
            {
                _webContext.NewCellSuggestions.Add(user);
            }

            _webContext.SaveChanges();
        }

        public void Remove(NewCellSuggestion user)
        {
           
            Save(user);
        }

        public void Remove(long userId)
        {
            var user = Get(userId);
            Remove(user);
        }
    }
}
