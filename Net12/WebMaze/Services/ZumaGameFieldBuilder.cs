using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.Services
{
    public class ZumaGameFieldBuilder
    {
        private UserService _userService;
        private Random random = new Random();

        public ZumaGameFieldBuilder(UserService userService)
        {
            _userService = userService;
        }

        public ZumaGameField Build(int width, int height, int colorCount)
        {

            var zumaGameField = new ZumaGameField()
            {
                Width = width,
                Height = height,
                Cells = new List<ZumaGameCell>(),
                Gamer = _userService.GetCurrentUser(),
                ColorCount = colorCount,
                IsActive = true
            };

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    var cell = new ZumaGameCell()
                    {
                        X = x,
                        Y = y,
                        Color = random.Next(1, colorCount),
                        IsActive = true
                    };

                    zumaGameField.Cells.Add(cell);

                }
            }

            return zumaGameField;
        }

    }
}