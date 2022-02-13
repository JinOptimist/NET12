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
        private SeaBattleEnemyCellRepository _seaBattleEnemyCellRepository;
        private SeaBattleMyCellRepository _seaBattleMyCellRepository;
        private Random _random = new Random();
        private int shipNumber;

        public SeaBattleService(UserService userService, 
                                SeaBattleEnemyCellRepository seaBattleEnemyCellRepository, 
                                SeaBattleMyCellRepository seaBattleMyCellRepository)
        {
            _userService = userService;
            _seaBattleEnemyCellRepository = seaBattleEnemyCellRepository;
            _seaBattleMyCellRepository = seaBattleMyCellRepository;
        }

        public SeaBattleGame CreateGame(SeaBattleDifficult difficult)
        {

            var myField = BuildMyField(difficult);
            var enemyField = BuildEnemyField(difficult);

            if(myField == null || enemyField == null)
            {
                return null;
            }

            var game = new SeaBattleGame
            {
                User = _userService.GetCurrentUser(),
                MyField = myField,
                EnemyField = enemyField
            };

            return game;
        }
        public SeaBattleMyField BuildMyField(SeaBattleDifficult difficult)
        {
            var field = new SeaBattleMyField()
            {
                Width = difficult.Width,
                Height = difficult.Height,
                Cells = new List<SeaBattleMyCell>()
            };

            shipNumber = 1;
            //for (int i = (int)ShipSize.Two; i <= (int)ShipSize.Four; i++)
            for (int i = 4; i >= 2; i--)
            {
                int attempt = 0;
                while (true)
                {
                    if (GenerateMyShips(field, difficult, i))
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
            field.ShipCount = shipNumber;

            for (int y = 0; y < difficult.Height; y++)
            {
                for (int x = 0; x < difficult.Width; x++)
                {
                    if (!field.Cells.Where(cell => cell.X == x && cell.Y == y).Any())
                    {
                        var cell = new SeaBattleMyCell()
                        {
                            X = x,
                            Y = y,
                            ShipLength = 0,
                            ShipHere = false,
                            Hit = false
                        };
                        field.Cells.Add(cell);
                    }
                }
            }

            return field;
        }

        private bool GenerateMyShips(SeaBattleMyField field, SeaBattleDifficult difficult, int shipSize)
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
                                var cell = new SeaBattleMyCell()
                                {
                                    X = startSpawnX - i,
                                    Y = startSpawnY,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false,
                                    ShipNumber = shipNumber,
                                    ShipDirection = direction
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                            shipNumber++;
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
                                var cell = new SeaBattleMyCell()
                                {
                                    X = startSpawnX + i,
                                    Y = startSpawnY,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false,
                                    ShipNumber = shipNumber,
                                    ShipDirection = direction
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                            shipNumber++;
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
                                var cell = new SeaBattleMyCell()
                                {
                                    X = startSpawnX,
                                    Y = startSpawnY - i,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false,
                                    ShipNumber = shipNumber,
                                    ShipDirection = direction
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                            shipNumber++;
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
                                var cell = new SeaBattleMyCell()
                                {
                                    X = startSpawnX,
                                    Y = startSpawnY + i,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false,
                                    ShipNumber = shipNumber,
                                    ShipDirection = direction
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                            shipNumber++;
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

        public SeaBattleEnemyField BuildEnemyField(SeaBattleDifficult difficult)
        {
            var field = new SeaBattleEnemyField()
            {
                Width = difficult.Width,
                Height = difficult.Height,
                Cells = new List<SeaBattleEnemyCell>()
            };

            shipNumber = 1;
            //for (int i = (int)ShipSize.Two; i <= (int)ShipSize.Four; i++)
            for (int i = 2; i <= 4; i++)
            {
                int attempt = 0;
                while (true)
                {
                    if (GenerateEnemyShips(field, difficult, i))
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

            field.ShipCount = shipNumber;

            for (int y = 0; y < difficult.Height; y++)
            {
                for (int x = 0; x < difficult.Width; x++)
                {
                    if (!field.Cells.Where(cell => cell.X == x && cell.Y == y).Any())
                    {
                        var cell = new SeaBattleEnemyCell()
                        {
                            X = x,
                            Y = y,
                            ShipLength = 0,
                            ShipHere = false,
                            Hit = false
                        };
                        field.Cells.Add(cell);
                    }
                }
            }

            return field;
        }

        private bool GenerateEnemyShips(SeaBattleEnemyField field, SeaBattleDifficult difficult, int shipSize)
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
                                var cell = new SeaBattleEnemyCell()
                                {
                                    X = startSpawnX - i,
                                    Y = startSpawnY,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false,
                                    ShipNumber = shipNumber,
                                    ShipDirection = direction
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                            shipNumber++;
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
                                var cell = new SeaBattleEnemyCell()
                                {
                                    X = startSpawnX + i,
                                    Y = startSpawnY,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false,
                                    ShipNumber = shipNumber,
                                    ShipDirection = direction
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                            shipNumber++;
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
                                var cell = new SeaBattleEnemyCell()
                                {
                                    X = startSpawnX,
                                    Y = startSpawnY - i,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false,
                                    ShipNumber = shipNumber,
                                    ShipDirection = direction
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                            shipNumber++;
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
                                var cell = new SeaBattleEnemyCell()
                                {
                                    X = startSpawnX,
                                    Y = startSpawnY + i,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false,
                                    ShipNumber = shipNumber,
                                    ShipDirection = direction
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                            shipNumber++;
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

        public void FillNearKilledEnemyShips(SeaBattleEnemyField enemyField)
        {

            for (int i = 1; i < enemyField.ShipCount; i++)
            {
                var ship = enemyField.Cells.Where(x => x.ShipNumber == i).ToList();
                var shipLenght = ship.First().ShipLength;

                if (shipLenght == ship.Where(x => x.Hit).Count())
                {
                    var hitNearShip = new List<SeaBattleEnemyCell>();

                    foreach (var cellShip in ship)
                    {
                        var baseNear = enemyField.Cells
                            .Where(cell =>
                                    (Math.Abs(cell.X - cellShip.X) <= 1 && Math.Abs(cell.Y - cellShip.Y) <= 1
                                    && !cell.ShipHere))
                            .ToList();

                        hitNearShip = hitNearShip.Union(baseNear).ToList();
                    }

                    foreach (var cellHit in hitNearShip)
                    {
                        if (!cellHit.Hit)
                        {
                            cellHit.Hit = true;
                            _seaBattleEnemyCellRepository.Save(cellHit);
                        }
                    }
                }
            }
        }

        public void FillNearKilledMyShips(SeaBattleMyField myField)
        {

            for (int i = 1; i < myField.ShipCount; i++)
            {
                var ship = myField.Cells.Where(x => x.ShipNumber == i).ToList();
                var shipLenght = ship.First().ShipLength;

                if (shipLenght == ship.Where(x => x.Hit).Count())
                {
                    var hitNearShip = new List<SeaBattleMyCell>();

                    foreach (var cellShip in ship)
                    {
                        var baseNear = myField.Cells
                            .Where(cell =>
                                    (Math.Abs(cell.X - cellShip.X) <= 1 && Math.Abs(cell.Y - cellShip.Y) <= 1
                                    && !cell.ShipHere))
                            .ToList();

                        hitNearShip = hitNearShip.Union(baseNear).ToList();
                    }

                    foreach (var cellHit in hitNearShip)
                    {
                        if (!cellHit.Hit)
                        {
                            cellHit.Hit = true;
                            _seaBattleMyCellRepository.Save(cellHit);
                        }
                    }
                }
            }
        }

    }
}
