using Net12.Maze;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.Models
{
    public class CellViewModelId
    {
        public long Id { get; set; }
        public BaseCell Cell { get; set; }

    }
}
