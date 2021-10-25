using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    public class BullEnemy : BaseEnemy
    {
        public BullEnemy(int x, int y, MazeLevel maze) : base(x, y, maze) { }

        public void Step()
        {

        }

        public override bool TryToStep()
        {
            Maze.Hero.Hp
            return false;
        }
    }
}
