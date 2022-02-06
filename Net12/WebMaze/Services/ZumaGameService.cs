using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;

namespace WebMaze.Services
{
    public class ZumaGameService
    {
        private UserService _userService;
        private ZumaGameCellRepository _zumaGameCellRepository;
        private ZumaGameFieldRepository _zumaGameFieldRepository;
        private Random random = new Random();

        public ZumaGameService(UserService userService, ZumaGameCellRepository zumaGameCellRepository, ZumaGameFieldRepository zumaGameFieldRepository)
        {
            _userService = userService;
            _zumaGameCellRepository = zumaGameCellRepository;
            _zumaGameFieldRepository = zumaGameFieldRepository;
        }

        public ZumaGameField BuildField(int width, int height, int colorCount)
        {

            var palette = Enum.GetValues(typeof(ZumaGameColors))
                .Cast<ZumaGameColors>()
                .Select(x => x.ToString())
                .ToList();

            var pal = new List<ZumaGameColor>();
            foreach (var color in palette)
            {
                pal.Add(new ZumaGameColor { Color = color });
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

        public bool PossibleClick(ZumaGameField field)
        {

            foreach (var currentCell in field.Cells)
            {
                if (field.Cells
                    .Where(cell => (cell.X == currentCell.X && Math.Abs(cell.Y - currentCell.Y) == 1
                    || Math.Abs(cell.X - currentCell.X) == 1 && cell.Y == currentCell.Y)
                    && cell.Color == currentCell.Color)
                    .Any())
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveCellsNearClicked(long id)
        {
            var getNear = _zumaGameCellRepository.GetNear(id);

            if (getNear.Count() > 1)
            {
                var field = _zumaGameCellRepository.Get(id).Field;
                var cells = field.Cells;

                foreach (var replaceCell in getNear)
                {
                    var replaceCells = cells.Where(db => db.Y < replaceCell.Y && db.X == replaceCell.X).ToList();

                    replaceCells.ForEach(x => x.Y++);

                    _zumaGameCellRepository.Remove(replaceCell);

                    _zumaGameCellRepository.UpdateCells(replaceCells);
                }

                for (int i = field.Width - 1; i >= 0; i--)
                {
                    var column = cells.Where(x => x.X == i).ToList();

                    if (column.Count() == 0)
                    {
                        for (int l = i + 1; l < field.Width; l++)
                        {
                            var cellsOffset = cells.Where(cell => cell.X == l).ToList();

                            cellsOffset.ForEach(cell => cell.X--);

                            _zumaGameCellRepository.UpdateCells(cellsOffset);
                        }
                    }
                }

                field.Score += (int)Math.Round(getNear.Count * Math.Pow(1.5, field.ColorCount - 2));
                _zumaGameFieldRepository.Save(field);
            }

        }
    }
}