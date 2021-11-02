using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    class VitalityPotion : Ground
    {

        public VitalityPotion(int x, int y, MazeLevel maze, int addMaxFatigue) : base(x, y, maze)
        {

            AddMaxFatigue = addMaxFatigue;

        }

        public int AddMaxFatigue { get; set; }

        public override bool TryToStep()
        {
            Maze.Hero.MaxFatigue += AddMaxFatigue;
            Maze[X, Y] = new Ground(X, Y, Maze);

            return true;
        }
    }
}
