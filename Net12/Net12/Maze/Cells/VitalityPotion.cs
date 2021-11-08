using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
   public class VitalityPotion : BaseCell
    {

        public VitalityPotion(int x, int y, IMazeLevel maze, int addMaxFatigue) : base(x, y, maze)
        {

            AddMaxFatigue = addMaxFatigue;

        }

        public int AddMaxFatigue { get; set; }

        public override bool TryToStep()
        {
            Maze.Hero.MaxFatigue += AddMaxFatigue;
            Maze.ReplaceCell(new Ground(X, Y, Maze));

            return true;
        }
    }
}
