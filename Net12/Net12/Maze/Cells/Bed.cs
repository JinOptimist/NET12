using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells
{
    class Bed: BaseCell
    {
        public override bool TryToStep()
        {
            Maze.Hero.CurrentFatigue;
            return true;
        }

    }
}
