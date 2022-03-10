using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.SeaBattle;
using WebMaze.EfStuff.Repositories;
using WebMaze.EfStuff.Repositories.SeaBattle;
using WebMaze.Models;
using WebMaze.SignalRHubs;

namespace WebMaze.Services
{
    public class SeaBattleService
    {
        private UserService _userService;
        private SeaBattleCellRepository _seaBattleCellRepository;
        private SeaBattleFieldRepository _seaBattleFieldRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private Random _random = new Random();
        private int shipNumber;

        public static List<SeaBattleTaskModel> SeaBattleTasks = new List<SeaBattleTaskModel>();
        private IHubContext<SeaBattleHub> _seaBattleHub;

        public SeaBattleService(UserService userService,
                                SeaBattleCellRepository seaBattleCellRepository,
                                SeaBattleFieldRepository seaBattleFieldRepository,
                                IHubContext<SeaBattleHub> seaBattleHub,
                                IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _seaBattleCellRepository = seaBattleCellRepository;
            _seaBattleFieldRepository = seaBattleFieldRepository;
            _seaBattleHub = seaBattleHub;
            _httpContextAccessor = httpContextAccessor;
        }

        public SeaBattleGame CreateGame(SeaBattleDifficult difficult)
        {

            var myField = BuildField(difficult, false);
            var enemyField = BuildField(difficult, true);

            var game = new SeaBattleGame()
            {
                User = _userService.GetCurrentUser(),
                Fields = new List<SeaBattleField>()
            };
            game.Fields.Add(myField);
            game.Fields.Add(enemyField);

            return game;
        }

        public SeaBattleField BuildField(SeaBattleDifficult difficult, bool isField)
        {
            var field = new SeaBattleField()
            {
                Width = difficult.Width,
                Height = difficult.Height,
                IsEnemyField = isField,
                LastHitToShip = -1,
                Cells = new List<SeaBattleCell>()
            };

            shipNumber = 1;
            //for (int i = (int)ShipSize.Two; i <= (int)ShipSize.Four; i++)
            for (int i = 4; i >= 2; i--)
            {
                GenerateMyShips(field, difficult, i);
            }
            field.ShipCount = shipNumber;

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
                            IsShip = false,
                            Hit = false
                        };
                        field.Cells.Add(cell);
                    }
                }
            }

            return field;
        }

        private void GenerateMyShips(SeaBattleField field, SeaBattleDifficult difficult, int shipSize)
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
                                        && x.IsShip)
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
                                    IsShip = true,
                                    Hit = false,
                                    ShipNumber = shipNumber
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                            shipNumber++;
                        }
                        break;

                    case 2:
                        startSpawnY = _random.Next(difficult.Height);
                        startSpawnX = _random.Next(difficult.Width - (shipSize - 1));

                        if (!field.Cells
                            .Where(x => x.X - startSpawnX >= -1
                                        && x.X - startSpawnX <= 4
                                        && Math.Abs(x.Y - startSpawnY) <= 1
                                        && x.IsShip)
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
                                    IsShip = true,
                                    Hit = false,
                                    ShipNumber = shipNumber
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                            shipNumber++;
                        }
                        break;

                    case 3:
                        startSpawnY = _random.Next(shipSize - 1, difficult.Height);
                        startSpawnX = _random.Next(difficult.Width);

                        if (!field.Cells
                            .Where(x => Math.Abs(x.X - startSpawnX) <= 1
                                        && startSpawnY - x.Y >= -1
                                        && startSpawnY - x.Y <= 4
                                        && x.IsShip)
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
                                    IsShip = true,
                                    Hit = false,
                                    ShipNumber = shipNumber
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                            shipNumber++;
                        }
                        break;

                    case 4:
                        startSpawnY = _random.Next(difficult.Height - (shipSize - 1));
                        startSpawnX = _random.Next(difficult.Width);

                        if (!field.Cells
                            .Where(x => Math.Abs(x.X - startSpawnX) <= 1
                                        && x.Y - startSpawnY >= -1
                                        && x.Y - startSpawnY <= 4
                                        && x.IsShip)
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
                                    IsShip = true,
                                    Hit = false,
                                    ShipNumber = shipNumber
                                };
                                field.Cells.Add(cell);
                            }
                            ships--;
                            shipNumber++;
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public void FillNearKilledShips(SeaBattleField field)
        {

            for (int i = 1; i < field.ShipCount; i++)
            {
                var ship = field.Cells.Where(x => x.ShipNumber == i).ToList();
                var shipLenght = ship.First().ShipLength;

                if (shipLenght == ship.Where(x => x.Hit).Count())
                {
                    var hitNearShip = new List<SeaBattleCell>();

                    foreach (var cellShip in ship)
                    {
                        var baseNear = field.Cells
                            .Where(cell =>
                                    (Math.Abs(cell.X - cellShip.X) <= 1
                                    && Math.Abs(cell.Y - cellShip.Y) <= 1
                                    && !cell.IsShip))
                            .ToList();

                        hitNearShip = hitNearShip.Union(baseNear).ToList();
                    }

                    foreach (var cellHit in hitNearShip)
                    {
                        if (!cellHit.Hit)
                        {
                            cellHit.Hit = true;
                            _seaBattleCellRepository.Save(cellHit);
                        }
                    }
                }
            }
        }

        public void RandomHit(SeaBattleField myField)
        {
            var cell = new SeaBattleCell();
            do
            {
                var hitX = _random.Next(myField.Width);
                var hitY = _random.Next(myField.Height);

                cell = myField.Cells.Where(x => !x.Hit && x.X == hitX && x.Y == hitY).SingleOrDefault();
            }
            while (cell == null);

            cell.Hit = true;

            _seaBattleCellRepository.Save(cell);

            if (cell.IsShip)
            {
                myField.LastHitToShip = cell.Id;
                _seaBattleFieldRepository.Save(myField);
            }
        }

        public void TryToDestroyShip(SeaBattleField myField)
        {
            var lastHitCell = _seaBattleCellRepository.Get(myField.LastHitToShip);

            var getNearCells = myField.Cells
                .Where(cell =>
                (cell.X == lastHitCell.X && Math.Abs(cell.Y - lastHitCell.Y) == 1
                || Math.Abs(cell.X - lastHitCell.X) == 1 && cell.Y == lastHitCell.Y))
                .ToList();

            var shipHere = getNearCells.Where(x => x.IsShip && x.Hit).ToList();
            if (shipHere.Any())
            {
                var secondHit = shipHere.First();
                if (lastHitCell.X == secondHit.X)
                {
                    bool isShoot = false;

                    //vertical ship down
                    for (int i = 1; i <= lastHitCell.ShipLength; i++)
                    {
                        var downCellToHit = myField.Cells
                            .Where(cell => cell.X == lastHitCell.X && cell.Y - lastHitCell.Y == i)
                            .SingleOrDefault();

                        if (downCellToHit != null)
                        {
                            if (downCellToHit.Hit)
                            {
                                if (!downCellToHit.IsShip)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                downCellToHit.Hit = true;
                                _seaBattleCellRepository.Save(downCellToHit);
                                isShoot = true;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (!isShoot)
                    {
                        //vertical ship up
                        for (int i = 1; i <= lastHitCell.ShipLength; i++)
                        {
                            var upCellToHit = myField.Cells
                                .Where(cell => cell.X == lastHitCell.X && lastHitCell.Y - cell.Y == i)
                                .SingleOrDefault();

                            if (upCellToHit != null)
                            {
                                if (upCellToHit.Hit)
                                {
                                    if (!upCellToHit.IsShip)
                                    {
                                        myField.LastHitToShip = -1;
                                        RandomHit(myField);
                                        break;
                                    }
                                }
                                else
                                {
                                    upCellToHit.Hit = true;
                                    _seaBattleCellRepository.Save(upCellToHit);
                                    break;
                                }
                            }
                            else
                            {
                                myField.LastHitToShip = -1;
                                RandomHit(myField);
                                break;
                            }
                        }
                    }
                }
                else if (lastHitCell.Y == secondHit.Y)
                {
                    bool isShoot = false;

                    //horizontal ship right
                    for (int i = 1; i <= lastHitCell.ShipLength; i++)
                    {
                        var rightCellToHit = myField.Cells
                            .Where(cell => cell.X - lastHitCell.X == i && cell.Y == lastHitCell.Y)
                            .SingleOrDefault();

                        if (rightCellToHit != null)
                        {
                            if (rightCellToHit.Hit)
                            {
                                if (!rightCellToHit.IsShip)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                rightCellToHit.Hit = true;
                                _seaBattleCellRepository.Save(rightCellToHit);
                                isShoot = true;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (!isShoot)
                    {
                        //horizontal ship left
                        for (int i = 1; i <= lastHitCell.ShipLength; i++)
                        {
                            var upCellToHit = myField.Cells
                                .Where(cell => lastHitCell.X - cell.X == i && cell.Y == lastHitCell.Y)
                                .SingleOrDefault();

                            if (upCellToHit != null)
                            {
                                if (upCellToHit.Hit)
                                {
                                    if (!upCellToHit.IsShip)
                                    {
                                        myField.LastHitToShip = -1;
                                        RandomHit(myField);
                                        break;
                                    }
                                }
                                else
                                {
                                    upCellToHit.Hit = true;
                                    _seaBattleCellRepository.Save(upCellToHit);
                                    break;
                                }
                            }
                            else
                            {
                                myField.LastHitToShip = -1;
                                RandomHit(myField);
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                var cellsToHit = getNearCells.Where(x => !x.Hit).ToList();
                var cellToHit = cellsToHit[_random.Next(cellsToHit.Count())];

                cellToHit.Hit = true;
                _seaBattleCellRepository.Save(cellToHit);
            }

        }

        public void StartTask(long id)
        {

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            SeaBattleTaskModel taskModel;

            var gameId = _userService.GetCurrentUser().SeaBattleGame.Id;

            lock (SeaBattleTasks)
            {
                if (!SeaBattleTasks.Any(x => x.Id == id))
                {
                    taskModel = new SeaBattleTaskModel
                    {
                        Id = id,
                        CancellationTokenSource = cancelTokenSource
                    };

                    SeaBattleTasks.Add(taskModel);

                    Task task = new Task(() => EnemyTurnTask(taskModel, gameId), token);

                    task.Start();
                }
            }
        }

        public void EnemyTurn(SeaBattleField myField)
        {

            if (myField.LastHitToShip > 0)
            {
                TryToDestroyShip(myField);
            }
            else
            {
                RandomHit(myField);
            }

            FillNearKilledShips(myField);
        }

        private void EnemyTurnTask(SeaBattleTaskModel taskModel, long gameId)
        {
            var timerIsActiveUser = 0;
            var secondsToEnemyTurn = 2;

            var client = new HttpClient();
            var path = _httpContextAccessor.HttpContext.Request.Host.ToUriComponent();

            while (timerIsActiveUser <= 100)
            {

                taskModel.CancellationTokenSource.Token.ThrowIfCancellationRequested();

                if (taskModel.UserIsActive)
                {
                    timerIsActiveUser = 0;
                    taskModel.UserIsActive = false;
                }

                if (secondsToEnemyTurn == 0)
                {
                    client.GetAsync("http://" + path + "/SeaBattle/EnemyTurn?gameId=" + gameId);
                    secondsToEnemyTurn = 2;
                }


                secondsToEnemyTurn--;
                timerIsActiveUser++;

                _seaBattleHub.Clients.All.SendAsync(gameId.ToString(), secondsToEnemyTurn);

                Thread.Sleep(1000);
            }

            lock (SeaBattleTasks)
            {
                SeaBattleTasks.Remove(taskModel);
            }
        }
    }
}
