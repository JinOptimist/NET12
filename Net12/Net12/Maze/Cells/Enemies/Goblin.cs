using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    public class Goblin : BaseEnemy
    {
        public Goblin(int x, int y, MazeLevel maze) : base(x, y, maze) { }

        private Random random = new Random();

         public override void Step()
        {
            var cellsAroundGoblin =
                Maze.Cells.Where(cell =>
                   Math.Abs(cell.X - X) <= 1 && Math.Abs(cell.Y - Y) <= 1).ToList();

            var cellsWhereGoblinCanStep =
                Maze.Cells.Where(cell =>
                    (cell.X == X && Math.Abs(cell.Y - Y) == 1 || cell.Y == Y && Math.Abs(cell.X - X) == 1)
                       && !(cell.X == Maze.Hero.X && cell.Y == Maze.Hero.Y))
                          .OfType<Ground>().ToList();

            if (cellsAroundGoblin.Any(x => x.X == Maze.Hero.X && x.Y == Maze.Hero.Y) && cellsWhereGoblinCanStep.Count > 0)
            {
                var newGoblinsPosition = GetRandom(cellsWhereGoblinCanStep);
                if(CharacterStep(newGoblinsPosition)){
                    X = newGoblinsPosition.X;
                    Y = newGoblinsPosition.Y;
                }
            }
        }

        public override bool TryToStep()
        {
            Maze[X, Y] = new Coin(X, Y, Maze, 10);
            Maze.Enemies.Remove(this);
            return false;
        }

        private BaseCell GetRandom(IEnumerable<BaseCell> cells)
        {
            var list = cells.ToList();
            var index = random.Next(list.Count);
            return list[index];
        }
    }
}
