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
            if (Maze.Hero.Money >= 1 && Maze.Hero.Hp < Maze.Hero.Max_hp)
            {
                Maze.Hero.Money /= 2;
                Maze.Hero.Hp = Maze.Hero.Max_hp;
               
            }
            return true;
        }
    }
}
