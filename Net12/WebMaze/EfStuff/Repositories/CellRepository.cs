using Net12.Maze;
using WebMaze.EfStuff.DbModel;
using Net12.Maze.Cells;
using System;

namespace WebMaze.EfStuff.Repositories
{
    public class CellRepository : BaseRepository<CellModel>
    {
        public CellRepository(WebContext webContext) : base(webContext)
        {
        }

        public BaseCell GetCurrentCell(CellModel cellModel, IMazeLevel maze)
        {
            switch (cellModel.TypeCell)
            {
                case CellInfo.Grow:
                    return new Ground(cellModel.X, cellModel.Y, maze);

                default: return new Wall(cellModel.X, cellModel.Y, maze);
            }
        }
        public CellModel GetCurrentCell(BaseCell cell, MazeLevelModel maze)
        {
            if(cell is Ground)
            {
                return new CellModel() { HpCell = 0, IsActive = true, X = cell.X, Y = cell.Y, TypeCell = CellInfo.Grow, MazeLevel = maze};
            }
            else
            {
                return new CellModel() { HpCell = 0, IsActive = true, X = cell.X, Y = cell.Y, TypeCell = CellInfo.Wall, MazeLevel = maze };
            }
        }
    }
}
