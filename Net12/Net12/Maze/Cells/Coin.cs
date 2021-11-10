using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    public class Coin : BaseCell
    {
        public Coin(int x, int y, IMazeLevel maze, int coinCount) : base(x, y, maze)
        {
            CoinCount = coinCount;
        }
        
        public int CoinCount { get; set; }

        public override bool TryToStep()
        {
            Maze.Hero.Money += CoinCount;
            Maze[X, Y] = new Ground(X, Y, Maze);
            return true;
        }
    }
}
