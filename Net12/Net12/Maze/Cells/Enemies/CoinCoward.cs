using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net12.Maze.Cells;

namespace Net12.Maze.Cells.Enemies
{
    class CoinCoward : BaseEnemy
    {
        public int CoinCount { get; set; }

        public CoinCoward(int x, int y, MazeLevel maze, int coinCount) : base(x, y, maze) 
        {
            CoinCount = coinCount;
        }

        private Random random = new Random();

        public override void Step()
        {
            var groundsNearCoinCoward = Maze.Cells
               .Where(cell =>
                      cell.X == X && Math.Abs(cell.Y - Y) == 1
                   || Math.Abs(cell.X - X) == 1 && cell.Y == Y)
                     .OfType<Ground>()
                     .ToList();
            

            foreach (Ground cell in groundsNearCoinCoward)
            {
                if (Maze.Hero.X == cell.X && Maze.Hero.Y == cell.Y )
                {

                }
            }




        }

        public override bool TryToStep()
        {
            Maze.Hero.Money += CoinCount;
            Maze[X, Y] = new Ground(X, Y, Maze);
            return true;
        }
    }
}
