using Net12.Maze.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12.Maze
{
    public class MazeBuilder
    {
        private MazeLevel maze;
        private Random random = new Random();

        public MazeLevel Build(int width, int height)
        {
            maze = new MazeLevel();

            maze.Width = width;
            maze.Height = height;

            BuildWall();

            BuildGround();

            BuildTeleport();
            
            

            var hero = new Hero(0, 0, maze);
            maze.Hero = hero;

            return maze;
        }

        private void BuildWall()
        {
            for (int y = 0; y < maze.Height; y++)
            {
                for (int x = 0; x < maze.Width; x++)
                {
                    var wall = new Wall(x, y, maze);
                    maze.Cells.Add(wall);
                }
            }
        }

        private void BuildGround()
        {
            var minerX = 0;
            var minerY = 0;

            var wallToBreak = new List<BaseCell>();

            do
            {
                maze[minerX, minerY] = new Ground(minerX, minerY, maze);

                var cell = maze[minerX, minerY];
                var nearWalls = GetNear<Wall>(cell);
                wallToBreak.AddRange(nearWalls);

                wallToBreak = wallToBreak.Where(cell => GetNear<Ground>(cell).Count() <= 1).ToList();
                if (!wallToBreak.Any())
                {
                    break;
                }
                var randomCell = GetRandom(wallToBreak);
                wallToBreak.Remove(randomCell);
                minerX = randomCell.X;
                minerY = randomCell.Y;
            } while (wallToBreak.Any());
        }

        private BaseCell GetRandom(List<BaseCell> cells)
        {
            var index = random.Next(cells.Count);

            return cells[index];
        }

        private List<TypeOfCell> GetNear<TypeOfCell>(BaseCell currentCell)
            where TypeOfCell : BaseCell
        {
            return maze.Cells
                .Where(cell => cell.X == currentCell.X && Math.Abs(cell.Y - currentCell.Y) == 1
                    || Math.Abs(cell.X - currentCell.X) == 1 && cell.Y == currentCell.Y)
                .OfType<TypeOfCell>()
                .ToList();
        }

        private void BuildTeleport()
        {
            var cellOut = new TeleportOut(3, 3, maze);
            maze[3, 3] = cellOut;
            maze[1, 1] = new TeleportIn(1, 1, maze, cellOut);
        }

    }
}
