using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    public class TeleportIn : BaseCell
    {
        public TeleportIn(int x, int y, IMazeLevel maze, ITeleportOut teleportExit) : base(x, y, maze) 
        {
            TeleportExit = teleportExit;
            
        }

        public ITeleportOut TeleportExit { get; set; }
     

        public override bool TryToStep()
        {
            Maze.Hero.X = TeleportExit.X;
            Maze.Hero.Y = TeleportExit.Y;
            return false;
        }
    }
}
