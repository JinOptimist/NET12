using Net12.Maze;
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

            BuildTrap();

            var hero = new Hero(0, 0, maze, 10);
            maze.Hero = hero;

            return maze;
        }

        private void BuildTrap()
        {
            var groundToTrap = maze.Cells.Where(x => x is Ground).ToList();
            var randomGroundToTrap = GetRandom(groundToTrap);

            var nearWalls = GetNear<Ground>(randomGroundToTrap);
            if(nearWalls.Count <= 2) maze[randomGroundToTrap.X, randomGroundToTrap.Y] = new Trap(randomGroundToTrap.X, randomGroundToTrap.Y, maze);

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
