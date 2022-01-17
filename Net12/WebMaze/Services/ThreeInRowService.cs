using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.ThreeInRow;

namespace WebMaze.Services
{
    public class ThreeInRowService
    {
        private UserService _userService;
        private Random random = new Random();

        public ThreeInRowService(UserService userService)
        {
            _userService = userService;
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
                        Color = colors[random.Next(0,6)],
                        GameField = gameField,
                        IsActive = true
                    };

                    gameField.Cells.Add(cell);
                }
            }

            return gameField;
        }
    }
}