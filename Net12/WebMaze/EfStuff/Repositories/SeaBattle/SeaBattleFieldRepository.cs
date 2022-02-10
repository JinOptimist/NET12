using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.DbModel.SeaBattle;

namespace WebMaze.EfStuff.Repositories
{
    public class SeaBattleFieldRepository<T,Y> : BaseRepository<T> where T: SeaBattleBaseField<Y>
                                                                    where Y: SeaBattleBaseCell
    {
        public SeaBattleFieldRepository(WebContext webContext) : base(webContext)
        {
        }
    }
}
