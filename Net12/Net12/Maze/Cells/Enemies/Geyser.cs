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
            Maze.Cells.Where(cell => cell.X == X && Math.Abs(cell.Y - Y) == 1
                                 || Math.Abs(cell.X - X) == 1 && cell.Y == Y)
                           .OfType<Ground>()
                           .ToList()
                           .ForEach(item => Maze[item.X, item.Y] = new Puddle(item.X, item.Y, Maze));
        }

    }
}
