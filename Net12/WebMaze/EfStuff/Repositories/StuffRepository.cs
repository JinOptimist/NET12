using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class StuffRepository: BaseRepository<StuffForHero>
    {
        public StuffRepository(WebContext webContext) : base(webContext)
        {
        }

        public override void Remove(StuffForHero model)
        {
            _dbSet.Remove(model);
            _webContext.SaveChanges();
        }
    }
}
