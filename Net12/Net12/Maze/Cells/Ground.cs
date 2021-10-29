using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    public class Ground : BaseCell
    {
        public Ground(int x, int y, IMazeLevel maze) : base(x, y, maze) { }

        public override bool TryToStep()
        {
            return true;
        }
    }
}
