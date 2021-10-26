using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    public class Slime : BaseEnemy
    {
        public int Hp { get; set; } = 1;
        public int X { get; set; }
        public int Y { get; set; }

        private Random random = new Random();

        public Slime(int x, int y, MazeLevel maze) : base(x, y, maze)
        {

        }

        public override void Step()
        {

            var isStep = Maze.Cells
                .Where(cell => cell.X == base.X && Math.Abs(cell.Y - base.Y) == 1
                    || Math.Abs(cell.X - base.X) == 1 && cell.Y == base.Y)
                .OfType<Ground>()
                .ToList();

            var newCoinX = base.X;
            var newCoinY = base.Y;

            if (isStep.Count > 0)
            {
                var toStep = isStep[random.Next(isStep.Count)];

                base.X = toStep.X;
                base.Y = toStep.Y;
            }
            else
            {
                var grounds = Maze.Cells.Where(x => x is Ground).ToList();
                var randomGround = GetRandom(grounds);
                base.X = randomGround.X;
                base.Y = randomGround.Y;

            }

            if (random.Next(0, 100) < 33)
            {
                Maze.Cells.Remove(base.Maze[newCoinX, newCoinY]);
                Maze.Cells.Add(new Coin(newCoinX, newCoinY, base.Maze, 5));
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