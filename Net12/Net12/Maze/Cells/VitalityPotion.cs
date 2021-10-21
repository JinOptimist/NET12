using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    class VitalityPotion : BaseCell
    {

        public VitalityPotion(int x, int y, MazeLevel maze, int addMaxFatigue) : base(x, y, maze)
        {

            valueVitalityPotion = addMaxFatigue;

        }

        public int valueVitalityPotion { get; set; }

        public override bool TryToStep()
        {
            Maze.Hero.MaxFatigue += valueVitalityPotion;
            Maze[X, Y] = new Ground(X, Y, Maze);

            return true;
        }
    }
}
