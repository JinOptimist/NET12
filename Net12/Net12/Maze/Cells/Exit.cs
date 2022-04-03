using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells
{
    public class Exit : BaseCell
    {
        public Exit(int x, int y, IMazeLevel maze) : base(x, y, maze)
        {
        } 

        public override bool TryToStep()
        {
            if (Maze.MessageExitStatus != "Exit is open")
            {
                return false;
            } 

                Maze.Message = "You won!";                
            
            return true;
        }
    }
}
