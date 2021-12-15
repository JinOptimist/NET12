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
            
            return GetAll(currentCell.Field)
                .Where(cell =>
                       Math.Abs(cell.Y - currentCell.Y) <= 1
                    && Math.Abs(cell.X - currentCell.X) <= 1
                    && cell.Color == currentCell.Color)
                .ToList();

        }

        public List<ZumaGameCell> GetAll(ZumaGameField field)
        {

            return field.Cells;
        }

        public void ReplaceCells(List<ZumaGameCell> cells)
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
