using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class MazeDifficultRepository
    {
        private WebContext _webContext;

        public MazeDifficultRepository(WebContext webContext)
        {
            _webContext = webContext;
        }

        public void Save(MazeDifficultProfile mazeDifficultProfile)
        {
            if (mazeDifficultProfile.Id > 0)
            {
                _webContext.Update(mazeDifficultProfile);
            }
            else
            {
                _webContext.MazeDifficultProfiles.Add(mazeDifficultProfile);
            }

            _webContext.SaveChanges();
        }

        public void Remove(MazeDifficultProfile mazeDifficultProfile)
        {
            _webContext.MazeDifficultProfiles.Remove(mazeDifficultProfile);
            _webContext.SaveChanges();
        }

        public List<MazeDifficultProfile> GetAll()
        {
            return _webContext
                .MazeDifficultProfiles
                .Where(x => x.IsActive)
                .ToList();
        }

    }
}
