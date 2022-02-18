using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.Services
{
    public class MinerFiledBuilder
    {
        private Random random = new Random();

        private UserService userService;

        public MinerFiledBuilder(UserService userService)
        {
            this.userService = userService;
        }

        public MinerField Build()
        {
            var width = 10;
            var height = 10;
            var minerFiled = new MinerField()
            {
                Height = height,
                Width = width,
                Cells = new List<MinerCell>(),
                Gamer = userService.GetCurrentUser(),
                IsActive = true,
            };

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var cell = new MinerCell()
                    {
                        X = x,
                        Y = y,
                        IsBomb = false,
                        IsOpen = false,
                        NearBombsCount = 0,
                        IsActive = true,
                        Field = minerFiled
                    };

                    minerFiled.Cells.Add(cell);

                }
            }

            for (int i = 0; i < 10; i++)
            {
                var notBombs = minerFiled.Cells.Where(x => !x.IsBomb).ToList();
                GetRandom(notBombs).IsBomb = true;
            }

            foreach (var cell in minerFiled.Cells.Where(x => !x.IsBomb))
            {
                cell.NearBombsCount = GetNear(minerFiled.Cells, cell)
                    .Where(x => x.IsBomb)
                    .Count();
            }

            return minerFiled;
        }

        public List<MinerCell> GetNear(List<MinerCell> cells, MinerCell currentCell)
        {
            return cells
                    .Where(cell => 
                       Math.Abs(cell.Y - currentCell.Y) <= 1 
                    && Math.Abs(cell.X - currentCell.X) <= 1)
                .ToList();
        }

        private MinerCell GetRandom(List<MinerCell> cells)
        {
            var index = random.Next(cells.Count);
            return cells[index];
        }
    }
}
