using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.DbModel.SeaBattle;

namespace WebMaze.EfStuff.Repositories.SeaBattle
{
    public class SeaBattleCellRepository<T> : BaseRepository<T> where T : SeaBattleBaseCell
    {
        public SeaBattleCellRepository(WebContext webContext) : base(webContext)
        {
        }
    }
}
