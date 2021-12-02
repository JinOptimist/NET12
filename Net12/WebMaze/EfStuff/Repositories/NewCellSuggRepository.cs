using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class NewCellSuggRepository : BaseRepository<NewCellSuggestion>
    {
        public NewCellSuggRepository(WebContext webContext) : base(webContext)
        {

        }
    }

}
