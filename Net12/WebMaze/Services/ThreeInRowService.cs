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

        string[] colors = { "none", "red", "green", "blue", "purple", "orange" };

        public ThreeInRowGameField Build()
        {
            var width = 6;
            var height = 6;
            var gameField = new ThreeInRowGameField()
            {
                Width = width,
                Height = height,
                Cells = new List<ThreeInRowCell>(),
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
                        Color = colors[0],
                        GameField = gameField,
                        IsActive = true
                    };

                    gameField.Cells.Add(cell);
                }
            }

            return gameField;
        }

        public string NextColor()
        {
            return colors[random.Next(1, 6)];
        }

        public void Step(long Id)
        {
            var gameField = _threeInRowGameFieldRepository.Get(4);
            var cell = gameField.Cells.SingleOrDefault(cell => cell.Id == Id);
            cell.Color = NextColor();            

            _threeInRowGameFieldRepository.Save(gameField);

            CheckStatus();
        }

        public void CheckStatus()
        {
            var gameField = _threeInRowGameFieldRepository.Get(4);

            var allCellToDelete = new List<ThreeInRowCell>();
            var cellToDeleteH = new List<ThreeInRowCell>();
            var cellToDeleteV = new List<ThreeInRowCell>();

            cellToDeleteH = checkHorizontal(gameField, cellToDeleteH);
            cellToDeleteV = checkVertical(gameField, cellToDeleteV);

            allCellToDelete.AddRange(cellToDeleteH);
            allCellToDelete.AddRange(cellToDeleteV);

            foreach (var item in allCellToDelete)
            {
                item.Color = colors[0];
            }

            _threeInRowGameFieldRepository.Save(gameField);
        }

        public List<ThreeInRowCell> checkHorizontal(ThreeInRowGameField gameField, List<ThreeInRowCell> cellsToDelete)
        {
            var cellToDeleteHorizontal = new List<ThreeInRowCell>();

            for (int y = 0; y < gameField.Height; y++)
            {
                for (int x = 0; x < gameField.Width; x++)
                {
                    var cell = gameField.Cells.SingleOrDefault(c => c.X == x && c.Y == y);
                    var nextCell = gameField.Cells.SingleOrDefault(c => c.X == x + 1 && c.Y == y);
                    var prevCell = gameField.Cells.SingleOrDefault(c => c.X == x - 1 && c.Y == y);

                    if (cell.Color != "none")
                    {
                        if (nextCell != null && cell.Color == nextCell.Color)
                        {
                            cellToDeleteHorizontal.Add(cell);
                        }
                        else if (prevCell != null && cell.Color == prevCell.Color)
                        {
                            cellToDeleteHorizontal.Add(cell);
                        }
                        
                    }

                    if (cellToDeleteHorizontal.Count > 2)
                    {
                        foreach (var item in cellToDeleteHorizontal)
                        {
                            cellsToDelete.Add(item);
                        }
                        cellsToDelete.Clear();
                    }

                }
            }

            return cellsToDelete;
        }

        public List<ThreeInRowCell> checkVertical(ThreeInRowGameField gameField, List<ThreeInRowCell> cellsToDelete)
        {
            var cellToDeleteVertical = new List<ThreeInRowCell>();

            for (int x = 0; x < gameField.Width; x++)
            {
                for (int y = 0; y < gameField.Height; y++)
                {
                    var cell = gameField.Cells.SingleOrDefault(c => c.X == x && c.Y == y);
                    var nextCell = gameField.Cells.SingleOrDefault(c => c.X == x && c.Y == y + 1);
                    var prevCell = gameField.Cells.SingleOrDefault(c => c.X == x && c.Y == y - 1);

                    if (cell.Color != "none")
                    {
                        if (nextCell != null && cell.Color == nextCell.Color)
                        {
                            cellToDeleteVertical.Add(cell);
                        }
                        else if (prevCell != null && cell.Color == prevCell.Color)
                        {
                            cellToDeleteVertical.Add(cell);
                        }

                    }

                    if (cellToDeleteVertical.Count > 2)
                    {
                        foreach (var item in cellToDeleteVertical)
                        {
                            cellsToDelete.Add(item);
                        }
                    }

                }
            }

            return cellsToDelete;
        }

    }
} 