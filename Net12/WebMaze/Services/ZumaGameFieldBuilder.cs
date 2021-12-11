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

            var palette = Enum.GetValues(typeof(ZumaGameColors))
                .Cast<ZumaGameColors>()
                .Select(x=>x.ToString())
                .ToList();

            var pal = new List<ZumaGameColor>();
            foreach (var color in palette)
            {
                pal.Add(new ZumaGameColor() {Color = color});
            }

            var removeColorCount = pal.Count - colorCount;

            for (int i = 0; i < removeColorCount; i++)
            {
                pal.RemoveAt(random.Next(pal.Count));
            }


            var zumaGameField = new ZumaGameField()
            {
                Width = width,
                Height = height,
                Cells = new List<ZumaGameCell>(),
                Gamer = _userService.GetCurrentUser(),
                ColorCount = colorCount,
                Palette = pal,
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
                        Color = GetRandom(pal).Color,
                        IsActive = true
                    };

                    zumaGameField.Cells.Add(cell);

                }
            }

            return zumaGameField;
        }

        public T GetRandom<T>(List<T> cells)
        {
            var index = random.Next(cells.Count);
            return cells[index];
        }

    }
}