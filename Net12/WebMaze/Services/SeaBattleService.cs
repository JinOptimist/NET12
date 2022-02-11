using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.SeaBattle;
using WebMaze.EfStuff.Repositories;
using WebMaze.EfStuff.Repositories.SeaBattle;

namespace WebMaze.Services
{
    public class SeaBattleService
    {
        private UserService _userService;
        private SeaBattleCellRepository _seaBattleCellRepository;
        private Random _random = new Random();

        public SeaBattleService(UserService userService, SeaBattleCellRepository seaBattleCellRepository)
        {
            _userService = userService;
            _seaBattleCellRepository = seaBattleCellRepository;
        }
        public enum ShipSize
        {
            Two = 2,
            Three = 3,
            Four = 4
        }

        public SeaBattleGame CreateGame(SeaBattleDifficult difficult)
        {
            var game = new SeaBattleGame();

            var myField = BuildField(difficult);
            var enemyField = BuildField(difficult);
            if(myField == null || enemyField == null)
            {
                return null;
            }
            //myField.Game = game;
            //enemyField.Game = game;

            game.Gamer = _userService.GetCurrentUser();
            game.MyField = myField;
            //game.EnemyField =new SeaBattleEnemyField
            //{
            //    Height = enemyField.Height,
            //    Width = enemyField.Width,
            //    Cells = enemyField.Cells
            //}

            return game;
        }
        public SeaBattleField BuildField(SeaBattleDifficult difficult)
        {

            var field = new SeaBattleField()
            {
                Width = difficult.Width,
                Height = difficult.Height,
                Cells = new List<SeaBattleCell>(),
                IsActive = true
            };

            //for (int i = (int)ShipSize.Two; i <= (int)ShipSize.Four; i++)
            for (int i = 2; i <= 4; i++)
            {
                int attempt = 0;
                while (true)
                {
                    if (GenerateShips(field, difficult, i))
                    {
                        break;
                    }
                    else
                    {
                        attempt++;
                    }

                    if (attempt >= 5)
                    {
                        return null;
                    }
                }
            }

            FillEmptyCells(field, difficult);

            return field;
        }

        private bool GenerateShips(SeaBattleField field, SeaBattleDifficult difficult, int shipSize)
        {
            int startSpawnY;
            int startSpawnX;
            int ships;

            switch (shipSize)
            {
                case 2:
                    ships = difficult.TwoSizeShip;
                    break;
                case 3:
                    ships = difficult.ThreeSizeShip;
                    break;
                case 4:
                    ships = difficult.FourSizeShip;
                    break;
                default:
                    ships = 0;
                    break;
            }

            var tryCount = 0;

            while (ships > 0)
            {
                // 1 - left 2 - right 3 - up 4 - down
                var direction = _random.Next(1, 5);

                switch (direction)
                {
                    case 1:
                        startSpawnY = _random.Next(difficult.Height);
                        startSpawnX = _random.Next(shipSize - 1, difficult.Width);

                        if (!field.Cells
                            .Where(x => startSpawnX - x.X >= -1
                                        && startSpawnX - x.X <= 4
                                        && Math.Abs(x.Y - startSpawnY) <= 1
                                        && x.ShipHere)
                            .ToList()
                            .Any())
                        {
                            for (int i = 0; i < shipSize; i++)
                            {
                                var cell = new SeaBattleCell()
                                {
                                    X = startSpawnX - i,
                                    Y = startSpawnY,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                        }
                        else
                        {
                            tryCount++;
                        }

                        break;
                    case 2:
                        startSpawnY = _random.Next(difficult.Height);
                        startSpawnX = _random.Next(difficult.Width - (shipSize - 1));

                        if (!field.Cells
                            .Where(x => x.X - startSpawnX >= -1
                                        && x.X - startSpawnX <= 4
                                        && Math.Abs(x.Y - startSpawnY) <= 1
                                        && x.ShipHere)
                            .ToList()
                            .Any())
                        {
                            for (int i = 0; i < shipSize; i++)
                            {
                                var cell = new SeaBattleCell()
                                {
                                    X = startSpawnX + i,
                                    Y = startSpawnY,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                        }
                        else
                        {
                            tryCount++;
                        }

                        break;
                    case 3:
                        startSpawnY = _random.Next(shipSize - 1, difficult.Height);
                        startSpawnX = _random.Next(difficult.Width);

                        if (!field.Cells
                            .Where(x => Math.Abs(x.X - startSpawnX) <= 1
                                        && startSpawnY - x.Y >= -1
                                        && startSpawnY - x.Y <= 4
                                        && x.ShipHere)
                            .ToList()
                            .Any())
                        {
                            for (int i = 0; i < shipSize; i++)
                            {
                                var cell = new SeaBattleCell()
                                {
                                    X = startSpawnX,
                                    Y = startSpawnY - i,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                        }
                        else
                        {
                            tryCount++;
                        }
                        break;
                    case 4:
                        startSpawnY = _random.Next(difficult.Height - (shipSize - 1));
                        startSpawnX = _random.Next(difficult.Width);

                        if (!field.Cells
                            .Where(x => Math.Abs(x.X - startSpawnX) <= 1
                                        && x.Y - startSpawnY >= -1
                                        && x.Y - startSpawnY <= 4
                                        && x.ShipHere)
                            .ToList()
                            .Any())
                        {
                            for (int i = 0; i < shipSize; i++)
                            {
                                var cell = new SeaBattleCell()
                                {
                                    X = startSpawnX,
                                    Y = startSpawnY + i,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                        }
                        else
                        {
                            tryCount++;
                        }
                        break;
                    default:
                        break;
                }

                if (tryCount >= 1000)
                {
                    return false;
                }
            }

            return true;
        }

        private void FillEmptyCells(SeaBattleField field, SeaBattleDifficult difficult)
        {
            for (int y = 0; y < difficult.Height; y++)
            {
                for (int x = 0; x < difficult.Width; x++)
                {
                    if (!field.Cells.Where(cell => cell.X == x && cell.Y == y).Any())
                    {
                        var cell = new SeaBattleCell()
                        {
                            X = x,
                            Y = y,
                            ShipLength = 0,
                            ShipHere = false,
                            Hit = false,
                            IsActive = true
                        };
                        field.Cells.Add(cell);
                    }
                }
            }
        }

    }
}
