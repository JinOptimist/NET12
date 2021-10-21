using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    public class Hero : BaseCell
    {
        public int Money { get; set; }

        public int Health { get; set; }

        public Hero(int x, int y, MazeLevel maze, int HeroHealth) : base(x, y, maze) {
            Health = HeroHealth;
        }

        public override bool TryToStep()
        {
            throw new NotImplementedException();
        }
    }
}
