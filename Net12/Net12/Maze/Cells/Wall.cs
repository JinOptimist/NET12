using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    class Wall : BaseCell
    {
        public Wall(int x, int y) : base(x, y) { }

        public override bool TryToStep()
        {
            return false;
        }
    }
}
