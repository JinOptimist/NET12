using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    class Healer : BaseCell
    {
        public Healer(int x, int y, MazeLevel maze) : base(x, y, maze)
        {
            
        }

        public override bool TryToStep()
        {
            if (Maze.Hero.Money >= 1 && Maze.Hero.Healt < Maze.Hero.MaxHealt)
            {
                Maze.Hero.Money /= 2;
                Maze.Hero.Healt = Maze.Hero.MaxHealt;
               
            }
            return true;
        }
    }
}
