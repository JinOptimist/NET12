using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    public class Slime : BaseEnemy
    {
        public int Hp { get; set; } = 1;

        private Random random = new Random();

        public Slime(int x, int y, MazeLevel maze) : base(x, y, maze)
        {

        }

        public override void Step()
        {

            var nearStep = Maze.Cells
                .Where(cell => cell.X == X && Math.Abs(cell.Y - Y) == 1
                    || Math.Abs(cell.X - X) == 1 && cell.Y == Y)
                .OfType<Ground>()
                .ToList();

            if (random.Next(0, 100) < 33)
            {
                Maze.Cells.Remove(Maze[X, Y]);
                Maze.Cells.Add(new Coin(X, Y, Maze, 5));
            }

            if (nearStep.Count > 0)
            {
                var toStep = nearStep[random.Next(nearStep.Count)];

                base.X = toStep.X;
                base.Y = toStep.Y;
            }
            else
            {
                var grounds = Maze.Cells.Where(x => x is Ground).ToList();
                var randomGround = GetRandom(grounds);
                X = randomGround.X;
                Y = randomGround.Y;

            }

        }

        public override bool TryToStep()
        {
            throw new NotImplementedException();
        }

        private BaseCell GetRandom(List<BaseCell> cells)
        {
            var index = random.Next(cells.Count);
            return cells[index];
        }
    }
}