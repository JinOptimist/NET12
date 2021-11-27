using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class FavGamesRepository : BaseRepository<Game>
    {
        public FavGamesRepository(WebContext webContext) : base(webContext)
        {
        }
    }
}
