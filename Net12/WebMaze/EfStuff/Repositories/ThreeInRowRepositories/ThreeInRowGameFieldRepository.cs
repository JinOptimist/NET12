using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.ThreeInRow;

namespace WebMaze.EfStuff.Repositories.ThreeInRowRepositories
{
    public class ThreeInRowGameFieldRepository : BaseRepository<ThreeInRowGameField>
    {
        public ThreeInRowGameFieldRepository(WebContext webContext) : base(webContext)
        {

        }
    }
}
