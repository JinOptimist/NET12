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

        public ThreeInRowService(UserService userService)
        {
            _userService = userService;
        }

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
                        GameField = gameField
                    };

                    gameField.Cells.Add(cell);
                }
            }

            return gameField;
        }
    }
}
