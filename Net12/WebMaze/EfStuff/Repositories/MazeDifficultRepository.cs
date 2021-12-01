using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class MazeDifficultRepository : BaseRepository<MazeDifficultProfile>
    {
        public MazeDifficultRepository(WebContext webContext) : base(webContext)
        {
        }
        public User GetRandomUser()
        {
            return _webContext.Users.First();
        }
    }

}
