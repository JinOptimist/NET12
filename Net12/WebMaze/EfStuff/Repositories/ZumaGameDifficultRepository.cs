using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class ZumaGameDifficultRepository : BaseRepository<ZumaGameDifficult>
    {
        public ZumaGameDifficultRepository(WebContext webContext) : base(webContext)
        {

        }
        public override void Remove(ZumaGameDifficult model)
        {
            _dbSet.Remove(model);
            _webContext.SaveChanges();
        }

    }

}
