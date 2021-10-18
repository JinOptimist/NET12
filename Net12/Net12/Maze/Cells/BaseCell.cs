using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    public abstract class BaseCell
    {
        public BaseCell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public abstract bool TryToStep();
    }
}
