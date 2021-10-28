using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Net12.Maze.Cells.Enemies
{
    public class Geyser : BaseEnemy
    {
        public Geyser(int x, int y, MazeLevel maze) : base(x, y, maze)
        {
        }

        public override void Step()
        {
            var geyser = Maze.GetCellOrUnit(X, Y);

            var nearCells = Maze.Cells.Where(cell => cell.X == geyser.X && Math.Abs(cell.Y - geyser.Y) == 1
                      || Math.Abs(cell.X - geyser.X) == 1 && cell.Y == geyser.Y)
                .OfType<Ground>()
                .ToList();

            foreach (var item in nearCells)
            {
                Maze.Cells.Remove(Maze[item.X, item.Y]);
                Maze.Cells.Add(new Puddle(item.X, item.Y, Maze));
            }
        }

        public override bool TryToStep()
        {
            return false;
        }
    }
}
