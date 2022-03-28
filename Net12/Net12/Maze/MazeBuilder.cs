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

        public MazeLevel Build(int width, int height, int hp, int maxHp, Action<int> getCoins = null, bool onlyWall = false)
        {
            maze = new MazeLevel();

            maze.GetCoins = getCoins;

            maze.Width = width;
            maze.Height = height;

            var hero = new Hero(0, 0, maze, hp, maxHp);
            maze.Hero = hero;

            BuildWall();
            BuildGround();

            if (!onlyWall)
            {
                BuildExit();
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
                BuildBullEnemy();
                BuildGeyser();
                BuildWallworm();
                BuildSlime();
                BuildWalker();
                BuildGoblin();
            }

            return maze;
        }

        private void BuildGeyser()
        {
            int amountGeyser = (maze.Width * maze.Height) / 250;
            for (int i = 0; i <= amountGeyser; i++)
            {
                var grounds = maze.Cells.Where(x => x is Ground).ToList();
                var randomGround = GetRandom(grounds);
                maze.Enemies.Add(new Geyser(randomGround.X, randomGround.Y, maze));
            }
        }

        private void BuildSlime()
        {
            int amountSlime = (maze.Width * maze.Height) / 450;
            for (int i = 0; i <= amountSlime; i++)
            {
                var grounds = maze.Cells.Where(x => x is Ground).ToList();
                var randomGround = GetRandom(grounds);
                maze.Enemies.Add(new Slime(randomGround.X, randomGround.Y, maze));
            }
        }

        private void BuildFountain()
        {
            int amountFountain = (maze.Width * maze.Height) / 450;
            for (int i = 0; i <= amountFountain; i++)
            {
                var grounds = maze.Cells.Where(x => x is Ground).ToList();
                //grounds.Remove(Hero.X, Hero.Y,);
                var randomGround = GetRandom(grounds);
                maze[randomGround.X, randomGround.Y] = new Fountain(randomGround.X, randomGround.Y, maze);
            }
        }

        private void BuildGoblin()
        {
            int amountGoblin = (maze.Width * maze.Height) / 200;
            for (int i = 0; i <= amountGoblin; i++)
            {
                var grounds = maze.Cells.Where(x => x is Ground).ToList();
                var randomGround = GetRandom(grounds);
                var g = new Goblin(randomGround.X, randomGround.Y, maze);
                maze.Enemies.Add(g);
            }
        }

        private void BuildBed()
        {
            int amountBed = (maze.Width * maze.Height) / 200;
            for (int i = 0; i <= amountBed; i++)
            {
                var grounds = maze.Cells.Where(x => x is Ground).ToList();
                var randomGround = GetRandom(grounds);
                maze[randomGround.X, randomGround.Y] = new Bed(randomGround.X, randomGround.Y, maze);
            }
        }

        private void BuildCoin()
        {
            int amountCoin = (maze.Width * maze.Height) / 100;
            for (int i = 0; i <= amountCoin; i++)
            {
                var grounds = maze.Cells.Where(x => x is Ground).ToList();
                var randonGround = GetRandom(grounds);
                maze[randonGround.X, randonGround.Y] = new Coin(randonGround.X, randonGround.Y, maze, 3);
            }
        }

        private void BuildWalker()
        {
            int amountWalker = (maze.Width * maze.Height) / 150;
            for (int i = 0; i <= amountWalker; i++)
            {
                var list = maze.Cells.Where(point => point is Ground).ToList();
                var point = GetRandom(list);
                var enemy = new Walker(point.X, point.Y, point.Maze);
                maze.Enemies.Add(enemy);
            }
        }

        private void BuildBless()
        {
            var res_point = maze.Cells.Where(point => GetNear<Wall>(point).Count == 3 && GetNear<BaseCell>(point).Count == 4).ToList();

            if (res_point != null)
            {
                var randomGround = GetRandom(res_point);
                maze[randomGround.X, randomGround.Y] = new Bless(randomGround.X, randomGround.Y, maze);

            }
        }

        private void BuildVitalityPotion()
        {
            int amountVitalityPotion = (maze.Width * maze.Height) / 250;
            for (int i = 0; i <= amountVitalityPotion; i++)
            {
                var grounds = maze.Cells.Where(x => x is Ground).ToList();
                var randomGround = GetRandom(grounds);
                maze[randomGround.X, randomGround.Y] = new VitalityPotion(randomGround.X, randomGround.Y, maze, 5);
            }
        }

        private void BuildTrap()
        {
            int amountTrap = (maze.Width * maze.Height) / 20;
            for (int i = 0; i < amountTrap; i++)
            {
                var grounds = maze.Cells.Where(x => x is Ground).ToList();
                grounds = grounds.Where(x => GetNear<Ground>(x).Count >= 2).ToList();

                if (grounds.Any())
                {
                    var groundToTrap = GetRandom(grounds);
                    maze[groundToTrap.X, groundToTrap.Y] = new Trap(groundToTrap.X, groundToTrap.Y, maze);
                }
            }
        }

        private void BuildTavern()
        {
            int amountTavern = (maze.Width * maze.Height) / 180;
            for (int i = 0; i <= amountTavern; i++)
            {
                var grounds = maze.Cells.Where(x => x is Ground).ToList();
                var randomGround = GetRandom(grounds);
                maze[randomGround.X, randomGround.Y] = new Tavern(randomGround.X, randomGround.Y, maze);
            }
        }

        private void BuildPudder()
        {
            int amountPudder = (maze.Width * maze.Height) / 40;
            for (int i = 0; i <= amountPudder; i++)
            {
                var grounds = maze.Cells.Where(x => x is Ground).ToList();

                var randomGround = GetRandom(grounds);
                maze[randomGround.X, randomGround.Y] = new Puddle(randomGround.X, randomGround.Y, maze);
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
            int amountHeaiPotion = (maze.Width * maze.Height) / 250;
            for (int i = 0; i <= amountHeaiPotion; i++)
            {
                var grounds = maze.Cells.Where(x => x is Ground).Where(x => (x.X != maze.Hero.X && x.Y != maze.Hero.Y)).ToList();
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
            int amountWolfPit = (maze.Width * maze.Height) / 50;
            for (int i = 0; i <= amountWolfPit; i++)
            {
                var groundCenter = maze.Cells.Where(cell => GetNear<Ground>(cell).Count() == 4).ToList();

                if (groundCenter == null)
                {
                    return;
                }
                var randomGround = GetRandom(groundCenter);
                maze[randomGround.X, randomGround.Y] = new WolfPit(randomGround.X, randomGround.Y, maze);
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

        private void BuildWallworm()
        {
            var wall = maze.Cells.Where(x => x is Wall && !(x is GoldMine)).ToList();
            var randomWall = GetRandom(wall);
            maze.Enemies.Add(new Wallworm(randomWall.X, randomWall.Y, maze));
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

        private void BuildBullEnemy()
        {
            int amountBullEnemy = (maze.Width * maze.Height) / 120;
            for (int i = 0; i <= amountBullEnemy; i++)
            {
                var grounds = maze.Cells.OfType<Ground>().Cast<BaseCell>().ToList();
                var randomGround = GetRandom(grounds);
                maze.Enemies.Add(new BullEnemy(randomGround.X, randomGround.Y, maze));
            }
        }

        private void BuildExit()
        {
            var grounds = maze.Cells.Where(x => x is Ground).ToList();
            var lastGround = grounds.Last();
            maze[lastGround.X, lastGround.Y] = new Exit(lastGround.X, lastGround.Y, maze);
        }
    }
}