using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    public class HealPotion : BaseCell
    {
        public HealPotion(int x, int y, MazeLevel maze) : base(x, y, maze) { }

        public override bool TryToStep()
        {
            if (Maze.Hero.Hp != Maze.Hero.Max_hp)                     
                Maze.Hero.Hp = Maze.Hero.Hp + 10;
            
            Maze[X, Y] = new Ground(X, Y, Maze);
            return true;
        }
    }
}
