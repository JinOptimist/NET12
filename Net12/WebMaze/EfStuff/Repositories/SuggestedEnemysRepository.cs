using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.Repositories
{
    public class SuggestedEnemysRepository
    {
        private WebContext _webContext;

        public SuggestedEnemysRepository(WebContext webContext)
        {
            _webContext = webContext;
        }

        public SuggestedEnemysRepository Get(long id)
        {
            return _webContext.SuggestedEnemys.SingleOrDefault(x => x.Id);
        }

        
    }
}
