using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    public class Wallworm : BaseEnemy
    {
        public Wallworm(int x, int y, MazeLevel maze) : base(x, y, maze)
        {
        }
        public int CounterStep { get; set; } = 0;
        public int StepsBeforeEating { get; set; } = 1;

        public override void Step()
        {
            CounterStep++;
            if (CounterStep > StepsBeforeEating)
            {
                EatingTheWall();
            }

        }

        public override bool TryToStep()
        {
            return false;
        }

        public void EatingTheWall()
        {
            var allWall = Maze.Cells.Where(x => x is Wall && !(x is WeakWall) && !(x is GoldMine)).ToList();
            if (allWall.Count > 0)
            {
                var randomWall = GetRandom(allWall);
                Maze[randomWall.X, randomWall.Y] = new WeakWall(randomWall.X, randomWall.Y, Maze);
            }
            CounterStep = 0;
        }
        BaseCell GetRandom(List<BaseCell> cells)
        {
            Random random = new Random();
            var index = random.Next(cells.Count);
            return cells[index];
        }

    }
}
