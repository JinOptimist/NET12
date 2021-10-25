using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells
{
    public class Tavern : BaseCell
    {
        public Tavern(int x, int y, MazeLevel maze, int coinCount) : base(x, y, maze) { }

        public override bool TryToStep()
        {
            Maze.Hero.Money -= 2;
            Maze.Hero.CurrentFatigue -= 5;
            return true;
        }
    }
}
