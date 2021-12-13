using Net12.Maze;
using WebMaze.EfStuff.DbModel;
using Net12.Maze.Cells;
using System;

namespace WebMaze.EfStuff.Repositories
{
    public class CellRepository : BaseRepository<MazeCellWeb>
    {
        public CellRepository(WebContext webContext) : base(webContext)
        {
        }

    }
}
