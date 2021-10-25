using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells
{
    public class Hero : BaseCell
    {
        public int Money { get; set; }

        public int Health { get; set; }

        public Hero(int x, int y, MazeLevel maze, int health) : base(x, y, maze)
        {
            Health = health;
        }

        public Hero(int x, int y, MazeLevel maze) : base(x, y, maze)
        {
        }

        public override bool TryToStep()
        {
            throw new NotImplementedException();
        }
    }
}
