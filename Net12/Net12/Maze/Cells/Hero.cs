﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells
{
    public class Hero : BaseCell
    {
        public int Money { get; set; }

        public int Health { get; set; }

        public Hero(int x, int y, MazeLevel maze, int heroHealth) : base(x, y, maze) {
            Health = heroHealth;
        }

        public override bool TryToStep()
        {
            throw new NotImplementedException();
        }
    }
}
