using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    class TeleportIn : BaseCell
    {
        public TeleportIn(int x, int y, MazeLevel maze, int numbers) : base(x, y, maze)
        {
            Numbers = numbers;
        }

        public int Numbers { get; set; }

        public override bool TryToStep()
        {
           
            return true;
        }
    }
}
