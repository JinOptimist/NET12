using Net12.Maze.Cells;
using Net12.Maze.Cells.Enemies;
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

        public MazeLevel Build(int width, int height, int hp, int maxHp)
        {
            maze = new MazeLevel();

            maze.Width = width;
            maze.Height = height;

            var hero = new Hero(0, 0, maze, hp, maxHp);
            maze.Hero = hero;
            var wallworm = new Wallworm(-1, -1, maze);
            maze.Wallworm = wallworm;
            BuildWall();
            BuildGround();
            BuildWolfPit();
            BuildGoldMine();
            BuildBed();
            BuildPudder();
            BuildVitalityPotion();
            BuildHealPotion();
            BuildTeleport();
            BuildHeler();
            BuildCoin();
            BuildWeakWalls();
            BuildTavern();
            BuildBless();
            BuildTrap();
            BuildFountain();
            BuildBed();

            return maze;
        }


        private void BuildFountain()
        {
            var grounds = maze.Cells.Where(x => x is Ground).ToList();
            var randomGround = GetRandom(grounds);
            maze[randomGround.X, randomGround.Y] = new Fountain(randomGround.X, randomGround.Y, maze);

        }

        private void BuildBed()
        {
            var grounds = maze.Cells.Where(x => x is Ground).ToList();
            var randomGround = GetRandom(grounds);
            maze[randomGround.X, randomGround.Y] = new Bed(randomGround.X, randomGround.Y, maze);
        }

        private void BuildCoin()
        {
            var grounds = maze.Cells.Where(x => x is Ground).ToList();
            var randonGround = GetRandom(grounds);
            maze[randonGround.X, randonGround.Y] = new Coin(randonGround.X, randonGround.Y, maze, 3);
        }
        
        private void BuildBless()
        {
            var res_point = maze.Cells.FirstOrDefault(point => GetNear<Wall>(point).Count == 3 && GetNear<BaseCell>(point).Count == 4);

            if (res_point != null)
            {
                maze[res_point.X, res_point.Y] = new Bless(res_point.X, res_point.Y, maze);

            }
        }

        private void BuildVitalityPotion()
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

        private void BuildTavern()
        {
            var grounds = maze.Cells.Where(x => x is Ground).ToList();
            var randomGround = GetRandom(grounds);
            maze[randomGround.X, randomGround.Y] = new Tavern(randomGround.X, randomGround.Y, maze);
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
        
        private void BuildHeler()
        {
            int amountHealer = (maze.Width * maze.Height) / 400;

            for (int i = 0; i <= amountHealer; i++)
            {
                var grounds = maze.Cells.Where(x => x is Ground).ToList();
                var randomGrounds = GetRandom(grounds);
                maze[randomGrounds.X, randomGrounds.Y] = new Healer(randomGrounds.X, randomGrounds.Y, maze);

            }
        }

        private void BuildGoldMine()
        {
            var currentPlaceToBuildGoldMine = maze.Cells.Where(cell => cell is Wall).ToList();
            double chanceToBuild = (currentPlaceToBuildGoldMine.Count * 0.1);
            for (var i = 0; i < chanceToBuild; i++)
            {
                var placeToBuildGoldMine = GetRandom(currentPlaceToBuildGoldMine);
                maze[placeToBuildGoldMine.X, placeToBuildGoldMine.Y] = new GoldMine(placeToBuildGoldMine.X, placeToBuildGoldMine.Y, maze);
            }
        }

        private void BuildHealPotion()
        {
            var grounds = maze.Cells.Where(x => x is Ground).Where(x => (x.X != maze.Hero.X && x.Y != maze.Hero.Y)).ToList();
            for (int i = 0; i < 3; i++)
            {
                var randomGround = GetRandom(grounds);
                maze[randomGround.X, randomGround.Y] = new HealPotion(randomGround.X, randomGround.Y, maze);

            }
        }
        
        private void BuildTeleport()
        {
            var grounds = maze.Cells.OfType<Ground>().Cast<BaseCell>().ToList();
            if (grounds.Count < 2)
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

        private void BuildWolfPit()
        {
            {
                var groundCenter = maze.Cells.FirstOrDefault(cell => GetNear<Ground>(cell).Count() == 4);

                if (groundCenter == null)
                {
                    return;
                }
                maze[groundCenter.X, groundCenter.Y] = new WolfPit(groundCenter.X, groundCenter.Y, maze);
            }
        }

        private void BuildWeakWalls()
        {


            var wallsOfMaze = maze.Cells.OfType<Wall>().Cast<BaseCell>().ToList();
            var wallToCheckForFourWall = wallsOfMaze.Where(cell => GetNear<Wall>(cell).Count <= 2).Cast<BaseCell>().ToList();
            var countOfWallInTheMaze = wallsOfMaze.Count;
            var countOfWeakWall = Math.Round(countOfWallInTheMaze / 10.0);


            for (int i = 0; countOfWeakWall > i; i++)
            {
                var randomWall = GetRandom(wallToCheckForFourWall);
                wallsOfMaze.Remove(randomWall);
                maze[randomWall.X, randomWall.Y] = new WeakWall(randomWall.X, randomWall.Y, maze);
                countOfWeakWall--;
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

       
    }
}
