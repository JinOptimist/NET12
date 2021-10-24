using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    public class GoldMine : BaseCell
    {
        public int GoldMineHp = 2;
        public int CurrentGoldMineMp = 0;

        public GoldMine(int x, int y, MazeLevel maze) : base(x, y, maze) { }

        public override bool TryToStep()
        {
            for (; CurrentGoldMineMp < GoldMineHp;)
            {
                CurrentGoldMineMp++;
                Maze.Hero.Money++;
                return false;
            }
            Maze[X, Y] = new Ground(X, Y, Maze);
            return false;

        }
    }
}
