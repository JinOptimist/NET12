using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class ZumaGameController : Controller
    {
        private ZumaGameFieldBuilder _zumaGameFieldBuilder;
        private ZumaGameFieldRepository _zumaGameFieldRepository;
        private ZumaGameCellRepository _zumaGameCellRepository;
        private IMapper _mapper;

        private int _width = 10;
        private int _height = 10;
        private int _colorCount = 4;

        public ZumaGameController(ZumaGameFieldBuilder zumaGameFieldBuilder, ZumaGameFieldRepository zumaGameFieldRepository, IMapper mapper, ZumaGameCellRepository zumaGameCellRepository)
        {
            _zumaGameFieldBuilder = zumaGameFieldBuilder;
            _zumaGameFieldRepository = zumaGameFieldRepository;
            _mapper = mapper;
            _zumaGameCellRepository = zumaGameCellRepository;
        }

        public IActionResult StartGame()
        {
            var filed = _zumaGameFieldBuilder.Build(_width, _height, _colorCount);

            _zumaGameFieldRepository.Save(filed);

            return RedirectToAction("Game", new { id = filed.Id });
        }
        public IActionResult Game(long id)
        {
            var field = _zumaGameFieldRepository.Get(id);
            var fieldViewModel = _mapper.Map<ZumaGameFieldViewModel>(field);

            return View(fieldViewModel);
        }

        public IActionResult ClickOnCell(long Id)
        {
            var cell = _zumaGameCellRepository.Get(Id);
            var field = _zumaGameFieldRepository.Get(cell.Field.Id);
            var cells = _zumaGameCellRepository.GetAll(field);

            var getNear = _zumaGameCellRepository.GetNear(cell);

            foreach (var replaceCell in getNear)
            {
                var replaceCells = cells.Where(db => db.Y < replaceCell.Y && db.X == replaceCell.X).ToList();

                replaceCells.Select(x => x.Y = x.Y + 1).ToList();

                replaceCells.Add(new EfStuff.DbModel.ZumaGameCell { 
                    Color = _zumaGameFieldBuilder.GetRandom(field.Palette).Color,
                    Field = field,
                    X = replaceCell.X,
                    Y = 0,
                    IsActive = true
                });

                _zumaGameCellRepository.Remove(replaceCell);
                _zumaGameCellRepository.ReplaceCells(replaceCells);

            }

            return RedirectToAction("Game", new { id = cell.Field.Id });
        }
    }
}
