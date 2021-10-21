using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    class Wall : BaseCell
    {
        public Wall(int x, int y, MazeLevel maze) : base(x, y, maze) { }

        public override bool TryToStep()
        {
            return false;
        }
    }
}
