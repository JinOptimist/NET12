using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    public abstract class BaseEnemy : BaseCell
    {
        public BaseEnemy(int x, int y, MazeLevel maze) : base(x, y, maze) 
        {
            X = x;
            Y = y;
        }

        public abstract void Step();
    }
}
