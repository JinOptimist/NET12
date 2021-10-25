using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    class Bless : BaseCell
    {
        public Bless(int x, int y, MazeLevel maze) : base(x,y,maze) { }

        public override bool TryToStep()
        {
            Maze.Hero.Hp = Maze.Hero.Max_hp;   
            return true;
        }
    }
}
