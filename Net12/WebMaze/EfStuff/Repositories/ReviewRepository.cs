using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class ReviewRepository : BaseRepository<Review>
    {
        public ReviewRepository(WebContext webContext) : base(webContext)
        {
        }
    }
}
