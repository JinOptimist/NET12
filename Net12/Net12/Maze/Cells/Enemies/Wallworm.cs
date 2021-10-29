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
        public int StepsBeforeEating { get; set; } = 3;

        public override void Step()
        {
            CounterStep++;
            if (CounterStep > StepsBeforeEating)
            {
                Random random = new Random();
                BaseCell GetRandom(List<BaseCell> cells)
                {
                    var index = random.Next(cells.Count);
                    return cells[index];
                }

                var grounds = Maze.Cells.Where(x => x is Wall).ToList();
                var randomGrounds = GetRandom(grounds);
                Maze[randomGrounds.X, randomGrounds.Y] = new Ground(randomGrounds.X, randomGrounds.Y, Maze);

                CounterStep = 0;
            }

        }

        public override bool TryToStep()
        {
            return true;
        }
    }
}
