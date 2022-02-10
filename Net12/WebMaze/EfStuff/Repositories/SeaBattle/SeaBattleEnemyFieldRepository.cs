using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.DbModel.SeaBattle;

namespace WebMaze.EfStuff.Repositories
{
    public class SeaBattleEnemyFieldRepository : BaseRepository<SeaBattleEnemyField>
    {
        public SeaBattleEnemyFieldRepository(WebContext webContext) : base(webContext)
        {
        }
    }
}
