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

        public MazeLevel Build(int width, int height, int hp, int max_hp)
        {
            maze = new MazeLevel();

            maze.Width = width;
            maze.Height = height;

            var hero = new Hero(0, 0, maze, hp, max_hp);
            maze.Hero = hero;

            BuildWall();

            BuildGround();

            BuildPudder();

            maze.Hero = hero;
            PlaceVitalityPotion();
            LocateHealPotion();


            BuildTeleport();

            BuildCoin();

            BuildBless();
            BuildTrap();

            maze.Hero = hero;


            return maze;
        }
        private void BuildBless()
        {
            var res_point = maze.Cells.FirstOrDefault(point => GetNear<Wall>(point).Count == 3 && GetNear<BaseCell>(point).Count == 4);

            if (res_point != null)
            {
                maze[res_point.X, res_point.Y] = new Bless(res_point.X, res_point.Y, maze);

            }
        }

        private void BuildCoin()
        {
            var grounds = maze.Cells.Where(x => x is Ground).ToList();
            var randomGround = GetRandom(grounds);
            maze[randomGround.X, randomGround.Y] = new Coin(randomGround.X, randomGround.Y, maze, 3);
        }

        private void PlaceVitalityPotion()
        {
            var grounds = maze.Cells.Where(x => x is Ground).ToList();
            var randomGround = GetRandom(grounds);
            maze[randomGround.X, randomGround.Y] = new VitalityPotion(randomGround.X, randomGround.Y, maze, 5);
        }

        private void BuildTrap()
        {
            var grounds = maze.Cells.Where(x => x is Ground).ToList();
            grounds = grounds.Where(x => GetNear<Ground>(x).Count >= 2).ToList();

            if (grounds.Any())
            {
                var groundToTrap = GetRandom(grounds);
                maze[groundToTrap.X, groundToTrap.Y] = new Trap(groundToTrap.X, groundToTrap.Y, maze);
            }
        }

        private void BuildPudder()
        {
            var grounds = maze.Cells.Where(x => x is Ground).ToList();

            var randomGround = GetRandom(grounds);
            maze[randomGround.X, randomGround.Y] = new Puddle(randomGround.X, randomGround.Y, maze);
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

       private void LocateHealPotion()
        {
            var grounds = maze.Cells.Where(x => x is Ground).Where(x => (x.X != maze.Hero.X && x.Y != maze.Hero.Y)).ToList();
            for (int i = 0; i < 3; i++)
            {
                var randomGround = GetRandom(grounds);              
                maze[randomGround.X, randomGround.Y] = new HealPotion(randomGround.X, randomGround.Y, maze);
            }
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
            var grounds = maze.Cells.OfType<Ground>().Cast<BaseCell>().ToList();
            if (grounds.Count<2)
            {
                return;
            }

            var randomGroundOut = GetRandom(grounds);
            var cellOut = new TeleportOut(randomGroundOut.X, randomGroundOut.Y, maze);
            maze[randomGroundOut.X, randomGroundOut.Y] = cellOut;

            grounds.Remove(cellOut);

            var randomGroundIn = GetRandom(grounds);                
            maze[randomGroundIn.X, randomGroundIn.Y] = new TeleportIn(randomGroundIn.X, randomGroundIn.Y, maze, cellOut);
        }
    }
}
