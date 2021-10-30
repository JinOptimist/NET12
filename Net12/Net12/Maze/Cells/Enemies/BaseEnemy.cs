using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    public abstract class BaseEnemy : BaseCell
    {
        public BaseEnemy(int x, int y, IMazeLevel maze) : base(x, y, maze) { }

        public abstract void Step();
    }
}
