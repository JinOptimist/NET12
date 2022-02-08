using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.SeaBattle;

namespace WebMaze.EfStuff.Repositories.SeaBattle
{
    public class SeaBattleMyCellRepository : BaseRepository<SeaBattleMyCell>
    {
        public SeaBattleMyCellRepository(WebContext webContext) : base(webContext)
        {
        }
    }
}
