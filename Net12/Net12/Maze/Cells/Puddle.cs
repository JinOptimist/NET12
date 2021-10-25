using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells
{
    public class Puddle : BaseCell
    {
        public Puddle(int x, int y, MazeLevel maze):base(x,y,maze)
        {
        }

        public override bool TryToStep()
        {
            Maze.Message = "wap wap";
            return true;
        }
    }
}
