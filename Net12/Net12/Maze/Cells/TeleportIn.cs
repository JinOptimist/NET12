using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    public class TeleportIn : BaseCell
    {
        public TeleportIn(int x, int y, MazeLevel maze, TeleportOut teleportExit) : base(x, y, maze) 
        {
            TeleportExit = teleportExit;
        }

        public TeleportOut TeleportExit { get; set; }

        public override bool TryToStep()
        {
            return true;
        }
    }
}
