﻿using System;
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
        private SeaBattleMyCellRepository _seaBattleCellRepository;
        private Random _random = new Random();

        public SeaBattleService(UserService userService, SeaBattleMyCellRepository seaBattleCellRepository)
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

            var myField = BuildField<SeaBattleMyField, SeaBattleMyCell>(difficult);
            //var enemyField = BuildField<SeaBattleEnemyField>(difficult);

            myField.Game = game;
            //enemyField.Game = game;

            game.MyField = myField;
            //game.EnemyField = enemyField;

            return game;
        }
        public T BuildField<T, Y>(SeaBattleDifficult difficult) where T : SeaBattleBaseField<Y>, new()
                                                                where Y : SeaBattleBaseCell, new()
        {

            var field = new T()
            {
                Width = difficult.Width,
                Height = difficult.Height,
                Cells = new List<Y>(),
                Game = new SeaBattleGame(),
                IsActive = true
            };

            //for (int i = (int)ShipSize.Two; i <= (int)ShipSize.Four; i++)
            for (int i = 2; i <= 4; i++)
            {
                int attempt = 0;
                while (true)
                {
                    if (GenerateShips<T, Y>(field, difficult, i))
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

            ////////////////////////??????????
            if (field.GetType() is SeaBattleEnemyField)
            {

            }
            //////////////////////////

            //FillEmptyCells<T, Y>(field, difficult);

            return field;
        }

        private bool GenerateShips<T, Y>(T field, SeaBattleDifficult difficult, int shipSize) where T : SeaBattleBaseField<Y>
                                                                                              where Y : SeaBattleBaseCell, new()
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
                // 0 - left 1 - right 2 - up 3 - down
                var direction = _random.Next(4);

                switch (direction)
                {
                    case 0:
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
                                var cell = new Y()
                                {
                                    X = startSpawnX - i,
                                    Y = startSpawnY,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false,
                                    Direction = direction,
                                    SpawnNumber = ships
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
                    case 1:
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
                                var cell = new Y()
                                {
                                    X = startSpawnX + i,
                                    Y = startSpawnY,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false,
                                    Direction = direction,
                                    SpawnNumber = ships
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
                                var cell = new Y()
                                {
                                    X = startSpawnX,
                                    Y = startSpawnY - i,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false,
                                    Direction = direction,
                                    SpawnNumber = ships
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
                                var cell = new Y()
                                {
                                    X = startSpawnX,
                                    Y = startSpawnY + i,
                                    ShipLength = shipSize,
                                    ShipHere = true,
                                    Hit = false,
                                    Direction = direction,
                                    SpawnNumber = ships
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

        private void FillEmptyCells<T, Y>(T field, SeaBattleDifficult difficult) where T : SeaBattleBaseField<Y>
                                                                                 where Y : SeaBattleBaseCell, new()
        {
            for (int y = 0; y < difficult.Height; y++)
            {
                for (int x = 0; x < difficult.Width; x++)
                {
                    var myCell = new Y()
                    {
                        X = x,
                        Y = y,
                        ShipLength = 0,
                        ShipHere = false,
                        Hit = false,
                        IsActive = true
                    };
                    field.Cells.Add(myCell);

                }
            }
        }

    }
}
