using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    class Coin : Ground
    {
        public Coin(int x, int y, MazeLevel maze, int coinCount) : base(x, y, maze)
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
