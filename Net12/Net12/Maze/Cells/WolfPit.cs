using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells
{
    public class WolfPit : BaseCell
    {
        public WolfPit(int x, int y, MazeLevel maze) : base(x, y, maze) { }


        public override bool TryToStep()
        {
            Maze.Hero.Health--;
            return true;
        }
    }
}

