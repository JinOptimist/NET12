using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class StuffForHeroRepository: BaseRepository<StuffForHero>
    {
        public StuffForHeroRepository(WebContext webContext) : base(webContext)
        {
        }
    }
}
