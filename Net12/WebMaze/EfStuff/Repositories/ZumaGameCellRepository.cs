using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class ZumaGameCellRepository : BaseRepository<ZumaGameCell>
    {
        public ZumaGameCellRepository(WebContext webContext) : base(webContext)
        {

        }

        public List<ZumaGameCell> GetNear(ZumaGameCell currentCell)
        {
            var i = true;
            var baseNear = currentCell.Field.Cells
                    .Where(cell =>
                    (cell.X == currentCell.X && Math.Abs(cell.Y - currentCell.Y) <= 1
                    || Math.Abs(cell.X - currentCell.X) <= 1 && cell.Y == currentCell.Y)
                    && cell.Color == currentCell.Color)
                    .ToList();

            var getNear = baseNear;
            foreach (var cellNear in baseNear)
            {
                var dd = getNear.Union(currentCell.Field.Cells
                    .Where(cell =>
                    (cell.X == cellNear.X && Math.Abs(cell.Y - cellNear.Y) <= 1
                    || Math.Abs(cell.X - cellNear.X) <= 1 && cell.Y == cellNear.Y)
                    && cell.Color == cellNear.Color)
                    .ToList()).ToList();
            }

            return getNear;
        }

        public void UpdateCells(List<ZumaGameCell> cells)
        {
            foreach (var model in cells)
            {
                _webContext.Update(model);
            }
            _webContext.SaveChanges();


        }

        public override void Remove(ZumaGameCell model)
        {
            _dbSet.Remove(model);
            _webContext.SaveChanges();
        }
    }

}
