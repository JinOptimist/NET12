using Net12.Maze.Cells.Enemies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells
{
    public class Geyser : BaseEnemy
    {
        public Geyser(int x, int y, MazeLevel maze) : base(x, y, maze)
        {
        }

        public override void Step()
        {
            throw new NotImplementedException();
        }

        public override bool TryToStep()
        {
            return false;
        }
    }
}
