using System;
using System.Collections.Generic;
using System.Linq;

namespace Net12.Maze.Cells.Enemies
{
    public class Wallworm : BaseEnemy
    {
        public Wallworm(int x, int y, MazeLevel maze) : base(x, y, maze)
        {
        }
        public int CounterStep { get; set; } = 0;
        public int StepsBeforeEating { get; set; } = 2;

        public override void Step()
        {
            CounterStep++;
            if (CounterStep > StepsBeforeEating)
            {
                Maze[X, Y] = new WeakWall(X, Y, Maze);
                StepNextWall();
                CounterStep = 0;
            }
        }

        public void StepNextWall()
        {
            var wallsNear = new List<BaseCell>();

            for (int i = X - 1; i < X + 2; i++)
            {
                for (int j = Y - 1; j < Y + 2; j++)
                {
                    var cell = Maze[i, j];
                    if (cell is Wall && !(cell is WeakWall))
                    {
                        wallsNear.Add(cell);
                    }
                }
            }

            if (wallsNear.Count != 0)
            {
                var randomWall = GetRandom(wallsNear);
                X = randomWall.X; Y = randomWall.Y;
            }
            else
            {
                var allWall = Maze.Cells.Where(x => x is Wall && !(x is WeakWall)).ToList();
                if (allWall.Count != 0)
                {
                    var randomWall = GetRandom(allWall);
                    X = randomWall.X; Y = randomWall.Y;
                }
            }
        }
        BaseCell GetRandom(List<BaseCell> cells)
        {
            Random random = new Random();
            var index = random.Next(cells.Count);
            return cells[index];
        }
    }
}
