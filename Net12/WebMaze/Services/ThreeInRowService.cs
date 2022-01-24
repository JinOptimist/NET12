using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.ThreeInRow;
using WebMaze.EfStuff.Repositories.ThreeInRowRepositories;

namespace WebMaze.Services
{
    public class ThreeInRowService
    {
        private UserService _userService;
        private ThreeInRowCellRepository _threeInRowCellRepository;
        private ThreeInRowGameFieldRepository _threeInRowGameFieldRepository;
        private Random random = new Random();

        public ThreeInRowService(UserService userService,
            ThreeInRowCellRepository threeInRowCellRepository,
            ThreeInRowGameFieldRepository threeInRowGameFieldRepository)
        {
            _userService = userService;
            _threeInRowCellRepository = threeInRowCellRepository;
            _threeInRowGameFieldRepository = threeInRowGameFieldRepository;
        }

        string[] colors = { "none", "red", "green", "blue", "purple", "orange" };

        public ThreeInRowGameField Build()
        {
            var width = 10;
            var height = 10;
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
            var gameField = _threeInRowGameFieldRepository.Get(1);
            var cell = gameField.Cells.SingleOrDefault(cell => cell.Id == Id);
            cell.Color = NextColor();

            _threeInRowGameFieldRepository.Save(gameField);
        }
    }
}