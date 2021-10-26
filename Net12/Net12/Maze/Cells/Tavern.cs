using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells
{
    public class Tavern : BaseCell
    {
        public Tavern(int x, int y, MazeLevel maze) : base(x, y, maze) { }

        public override bool TryToStep()
        {
            if (Maze.Hero.Money <= 0)
            {
                return false;
            }

            Maze.Hero.Money -= 1;

            if (Maze.Hero.CurrentFatigue <= 5)
            {
                Maze.Hero.CurrentFatigue = 0;
            }
            else
            {
                Maze.Hero.CurrentFatigue -= 5;
            }

            return true;
        }
    }
}
