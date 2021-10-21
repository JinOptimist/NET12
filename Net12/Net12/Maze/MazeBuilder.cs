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

            var hero = new Hero(0, 0, maze);
            maze.Hero = hero;

            BuildBless();
            

            return maze;
        }
        private void BuildBless()
        {
            bool check = true;
            for (int y = 0; y < maze.Width && check; y++)
            {
                for (int x = 0; x < maze.Height && check; x++)
                {
                    var count = maze.Cells.Where(point => Math.Abs(point.X - x) == 1 && point.Y ==y || Math.Abs(point.Y - y) == 1 && point.X ==x)
                                          .Where(point => point.TryToStep() == false).ToList().Count;
                    if (  count == 3 )
                    {
                        maze[x, y] = new Bless(x, y, maze);
                        check = false;
                    }
                }
            }
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


    }
}
