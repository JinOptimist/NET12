using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.SeaBattle;

namespace WebMaze.EfStuff.Repositories.SeaBattle
{
    public class SeaBattleDifficultRepository : BaseRepository<SeaBattleDifficult>
    {
        public SeaBattleDifficultRepository(WebContext webContext) : base(webContext)
        {
        }
    }
}
