using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells
{
    public class Hero : BaseCell
    {
        public int Money { get; set; }
        public int Healt { get; set; } = 100;

        public Hero(int x, int y, MazeLevel maze) : base(x, y, maze) { }

        public override bool TryToStep()
        {
            throw new NotImplementedException();
        }
    }
}
