using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.ThreeInRow;
using WebMaze.EfStuff.Repositories.ThreeInRowRepositories;
using AutoMapper;
using WebMaze.Models.ThreeInRow;

namespace WebMaze.Services
{
    public class ThreeInRowService
    {
        private UserService _userService;
        private ThreeInRowCellRepository _threeInRowCellRepository;
        private ThreeInRowGameFieldRepository _threeInRowGameFieldRepository;
        private IMapper _mapper;

        private Random random = new Random();

        public ThreeInRowService(UserService userService,
            ThreeInRowCellRepository threeInRowCellRepository,
            ThreeInRowGameFieldRepository threeInRowGameFieldRepository,
            IMapper mapper)
        {
            _userService = userService;
            _threeInRowCellRepository = threeInRowCellRepository;
            _threeInRowGameFieldRepository = threeInRowGameFieldRepository;
            _mapper = mapper;

        }

        string None = "none";
        string[] colors = { "red", "green", "blue", "purple", "orange" };

        public ThreeInRowGameField Build()
        {
            var width = 5;
            var height = 5;
            var gameField = new ThreeInRowGameField()
            {
                Width = width,
                Height = height,
                Score = 0,
                Cells = new List<ThreeInRowCell>(),
                NextColor = GetNextColor(),
                Player = _userService.GetCurrentUser(),
                IsActive = true
            };

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var cell = new ThreeInRowCell()
                    {
                        X = x,
                        Y = y,
                        Color = None,
                        GameField = gameField,
                        IsActive = true
                    };

                    gameField.Cells.Add(cell);
                }
            }

            return gameField;
        }

        public string GetNextColor()
        {
            return colors[random.Next(colors.Length)];
        }

        public void Step(long CellId, long GameId)
        {
            var gameField = _threeInRowGameFieldRepository.Get(GameId);
            var cell = gameField.Cells.SingleOrDefault(cell => cell.Id == CellId);
            cell.Color = gameField.NextColor;

            var randomCellList = gameField.Cells.Where(c => c.Id != CellId & c.Color == None).ToList();
            if (randomCellList.Any())
            {
                var randomCellIndex = random.Next(randomCellList.Count);
                var randomCell = randomCellList[randomCellIndex];
                randomCell.Color = GetNextColor();
            }


            gameField.NextColor = GetNextColor();

            _threeInRowGameFieldRepository.Save(gameField);

            CheckStatus(GameId);
        }

        public void CheckStatus(long GameId)
        {
            var gameField = _threeInRowGameFieldRepository.Get(GameId);

            var allCellToDelete = new List<ThreeInRowCell>();
            var cellToDeleteH = new List<ThreeInRowCell>();
            var cellToDeleteV = new List<ThreeInRowCell>();

            cellToDeleteH = checkHorizontal(gameField, cellToDeleteH).Distinct().ToList();
            cellToDeleteV = checkVertical(gameField, cellToDeleteV).Distinct().ToList();

            allCellToDelete = cellToDeleteH.Union(cellToDeleteV).ToList();

            foreach (var item in allCellToDelete)
            {
                item.Color = None;
            }

            gameField.Score += allCellToDelete.Count();

            _threeInRowGameFieldRepository.Save(gameField);
        }

        public List<ThreeInRowCell> checkHorizontal(ThreeInRowGameField gameField, List<ThreeInRowCell> cellsToDelete)
        {
            var cellToDeleteH = new List<ThreeInRowCell>();

            for (int y = 0; y < gameField.Height; y++)
            {
                for (int x = 0; x < gameField.Width; x++)
                {
                    var cell = gameField.Cells.SingleOrDefault(c => c.X == x && c.Y == y);
                    var prevCell = gameField.Cells.SingleOrDefault(c => c.X == x - 1 && c.Y == y);

                    if (cell.Color == None)
                    {
                        continue;
                    }

                    if (cell.X == 0 )
                    {
                        cellToDeleteH.Add(cell);
                        continue;
                    }

                    if (cell.Color == prevCell.Color)
                    {
                        cellToDeleteH.Add(cell);
                        continue;
                    }
                    if (cell.Color != prevCell.Color)
                    {
                        if (cellToDeleteH.Count > 2)
                        {
                            foreach (var item in cellToDeleteH)
                            {
                                cellsToDelete.Add(item);
                            }
                        }
                        cellToDeleteH.Clear();
                        cellToDeleteH.Add(cell);
                        continue;
                    }
                }
                if (cellToDeleteH.Count > 2)
                {
                    foreach (var item in cellToDeleteH)
                    {
                        cellsToDelete.Add(item);
                    }
                }
                cellToDeleteH.Clear();
            }
            return cellsToDelete;
        }

        public List<ThreeInRowCell> checkVertical(ThreeInRowGameField gameField, List<ThreeInRowCell> cellsToDelete)
        {
            var cellToDeleteV = new List<ThreeInRowCell>();

            for (int x = 0; x < gameField.Width; x++)
            {
                for (int y = 0; y < gameField.Height; y++)
                {
                    var cell = gameField.Cells.SingleOrDefault(c => c.X == x && c.Y == y);
                    var prevCell = gameField.Cells.SingleOrDefault(c => c.X == x && c.Y == y - 1);

                    if (cell.Color == None)
                    {
                        continue;
                    }

                    if (cell.Y == 0)
                    {
                        cellToDeleteV.Add(cell);
                        continue;
                    }

                    if (cell.Color == prevCell.Color)
                    {
                        cellToDeleteV.Add(cell);
                        continue;
                    }
                    if (cell.Color != prevCell.Color)
                    {
                        if (cellToDeleteV.Count > 2)
                        {
                            foreach (var item in cellToDeleteV)
                            {
                                cellsToDelete.Add(item);
                            }
                        }
                        cellToDeleteV.Clear();
                        cellToDeleteV.Add(cell);
                        continue;
                    }
                }
                if (cellToDeleteV.Count > 2)
                {
                    foreach (var item in cellToDeleteV)
                    {
                        cellsToDelete.Add(item);
                    }
                }
                cellToDeleteV.Clear();
            }
            return cellsToDelete;
        }

    }
}