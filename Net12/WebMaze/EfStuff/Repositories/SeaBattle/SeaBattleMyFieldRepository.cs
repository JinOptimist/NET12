using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.SeaBattle;

namespace WebMaze.EfStuff.Repositories
{
    public class SeaBattleMyFieldRepository: BaseRepository<SeaBattleMyField>
    {
        public SeaBattleMyFieldRepository(WebContext webContext) : base(webContext)
        {
        }
    }
}
