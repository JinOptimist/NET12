using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    public abstract class Character : BaseCell
    {
        public int Hp { get; set; }
        public Character(int x, int y, IMazeLevel maze) : base(x, y, maze)
        {
        }

        public override bool TryToStep()
        {
            throw new NotImplementedException();
        }
    }
}
