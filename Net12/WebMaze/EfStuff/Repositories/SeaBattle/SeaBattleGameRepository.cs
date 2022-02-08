using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.SeaBattle;

namespace WebMaze.EfStuff.Repositories.SeaBattle
{
    public class SeaBattleGameRepository : BaseRepository<SeaBattleGame>
    {
        public SeaBattleGameRepository(WebContext webContext) : base(webContext)
        {
        }
    }
}
