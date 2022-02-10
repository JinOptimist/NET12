using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class ZumaGameFieldRepository : BaseRepository<ZumaGameField>
    {
        public ZumaGameFieldRepository(WebContext webContext) : base(webContext)
        {

        }

        public override void Remove(ZumaGameField model)
        {
            _dbSet.Remove(model);
            _webContext.SaveChanges();
        }
    }

}
