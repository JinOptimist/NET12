using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells
{
    class Bed: BaseCell
    {
        public Bed(int x, int y, MazeLevel maze) : base(x, y, maze) { }
        public override bool TryToStep()
        {
            Maze.Hero.CurrentFatigue = 0;
            Maze[X, Y] = new Ground(X, Y, Maze);
            return true;
        }
    }
}
