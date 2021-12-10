using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
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

            List<string> actualColor = Enum.GetValues(typeof(ZumaGameColors))
                .Cast<ZumaGameColors>()
                .Select(v => v.ToString())
                .ToList();

            var needActualColor = actualColor.Count - colorCount;

            for (int i = 0; i < needActualColor; i++)
            {
                actualColor.RemoveAt(random.Next(actualColor.Count));
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    var cell = new ZumaGameCell()
                    {
                        X = x,
                        Y = y,
                        Color = actualColor[random.Next(colorCount)],
                        IsActive = true
                    };

                    zumaGameField.Cells.Add(cell);

                }
            }

            return zumaGameField;
        }

    }
}