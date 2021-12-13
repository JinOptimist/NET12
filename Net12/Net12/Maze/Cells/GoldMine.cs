using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    public class GoldMine : Wall
    {
        public int goldMineMaxHp { get; set; } = 3;
        public int currentGoldMineMp { get; set; } = 0;

        public GoldMine(int x, int y, IMazeLevel maze) : base(x, y, maze) { }

        public override bool TryToStep()
        {
            Maze.Hero.Money++;
            Maze.GetCoins(1);

            currentGoldMineMp++;

            if (currentGoldMineMp >= goldMineMaxHp)
            {
                Maze.ReplaceCell(new Ground(X, Y, Maze));
            }
            return false;
        }
    }
}
