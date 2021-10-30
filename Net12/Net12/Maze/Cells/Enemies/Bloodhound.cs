using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    class Bloodhound : BaseEnemy
    {
        public Bloodhound(int x, int y, MazeLevel maze) : base(x, y, maze)
        {
        }

        public override void Step()
        {
          
        }

        public override bool TryToStep()
        {
            return true;
        }
    }
}
