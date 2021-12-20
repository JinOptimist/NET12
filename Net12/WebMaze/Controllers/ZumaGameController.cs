using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;
using WebMaze.SignalRHubs;

namespace WebMaze.Controllers
{
    public class ZumaGameController : Controller
    {
        private ZumaGameFieldBuilder _zumaGameFieldBuilder;
        private ZumaGameFieldRepository _zumaGameFieldRepository;
        private ZumaGameCellRepository _zumaGameCellRepository;
        private IMapper _mapper;
        private IHubContext<ChatHub> _chatHub;
        private UserService _userService;

        /// <summary>
        /// ШИРИНА
        /// </summary>
        private int _width = 10;
        /// <summary>
        /// ВЫСОТА
        /// </summary>
        private int _height = 10;
        private int _colorCount = 3;

        public ZumaGameController(ZumaGameFieldBuilder zumaGameFieldBuilder, ZumaGameFieldRepository zumaGameFieldRepository, IMapper mapper, ZumaGameCellRepository zumaGameCellRepository, IHubContext<ChatHub> chatHub, UserService userService)
        {
            _zumaGameFieldBuilder = zumaGameFieldBuilder;
            _zumaGameFieldRepository = zumaGameFieldRepository;
            _mapper = mapper;
            _zumaGameCellRepository = zumaGameCellRepository;
            _chatHub = chatHub;
            _userService = userService;
        }

        public IActionResult StartGame()
        {
            var field = _zumaGameFieldBuilder.Build(_width, _height, _colorCount);

            _zumaGameFieldRepository.Save(field);

            return RedirectToAction("Game", new { id = field.Id });
        }
        public IActionResult Game(long id)
        {
            var field = _zumaGameFieldRepository.Get(id);
            if (field.Cells.Count() > 0)
            {
                var fieldViewModel = _mapper.Map<ZumaGameFieldViewModel>(field);

                return View(fieldViewModel);

            }
            else
            {
                return RedirectToAction("WinGame");
            }
        }

        public IActionResult ClickOnCell(long Id)
        {
            var cell = _zumaGameCellRepository.Get(Id);
            var cells = cell.Field.Cells;

            var getNear = _zumaGameCellRepository.GetNear(cell);

            foreach (var replaceCell in getNear)
            {
                var replaceCells = cells.Where(db => db.Y < replaceCell.Y && db.X == replaceCell.X).ToList();

                replaceCells.ForEach(x => x.Y++);

                _zumaGameCellRepository.Remove(replaceCell);

                _zumaGameCellRepository.UpdateCells(replaceCells);
            }

            for (int i = _width - 1; i >= 0; i--)
            {
                var column = cells.Where(x => x.X == i).ToList();

                if (column.Count() == 0)
                {
                    for (int l = i + 1; l < _width; l++)
                    {
                        var cellsOffset = cells.Where(cell => cell.X == l).ToList();

                        cellsOffset.ForEach(cell => cell.X--);

                        _zumaGameCellRepository.UpdateCells(cellsOffset);
                    }
                }
            }

            return RedirectToAction("Game", new { id = cell.Field.Id });
        }

        public IActionResult WinGame()
        {

            _chatHub.Clients.All.SendAsync("ZumaGameWin", _userService.GetCurrentUser());
            return View();

        }
    }
}
