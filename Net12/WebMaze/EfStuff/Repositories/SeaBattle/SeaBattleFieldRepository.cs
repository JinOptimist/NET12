using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.DbModel.SeaBattle;

namespace WebMaze.EfStuff.Repositories
{
    public class SeaBattleFieldRepository : BaseRepository<SeaBattleField>
    {
        public SeaBattleFieldRepository(WebContext webContext) : base(webContext)
        {
        }
    }
}
