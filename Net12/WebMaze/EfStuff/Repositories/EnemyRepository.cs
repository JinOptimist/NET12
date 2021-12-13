using Net12.Maze;
using WebMaze.EfStuff.DbModel;
using Net12.Maze.Cells;
using System;

namespace WebMaze.EfStuff.Repositories
{
    public class MazeEnemyRepository : BaseRepository<MazeEnemyWeb>
    {
        public MazeEnemyRepository(WebContext webContext) : base(webContext)
        {
        }

    }
}
