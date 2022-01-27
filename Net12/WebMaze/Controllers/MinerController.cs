using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class MinerController : Controller
    {

        MinerFieldRepository _minerFieldRepository;
        MinerFiledBuilder _minerFiledBuilder;
        MinerCellRepository _minerCellRepository;
        IMapper _mapper;

        public MinerController(MinerFieldRepository minerFieldRepository,
            MinerCellRepository minerCellRepository,
            MinerFiledBuilder minerFiledBuilder, IMapper mapper)
        {
            _minerFieldRepository = minerFieldRepository;
            _minerCellRepository = minerCellRepository;
            _minerFiledBuilder = minerFiledBuilder;
            _mapper = mapper;
        }

        public IActionResult StartGame()
        {
            var filed = _minerFiledBuilder.Build();
            _minerFieldRepository.Save(filed);

            return RedirectToAction("Game", new { id = filed.Id });
        }

        public IActionResult Game(long id)
        {
            var field = _minerFieldRepository.Get(id);
            if (field.IsOver)
            {
                foreach (var cell in field.Cells.Where(x => x.IsBomb && !x.IsFlag))
                {
                    cell.IsOpen = true;
                }
            }
            if (field.Cells.Where(x => !x.IsBomb).All(x => x.IsOpen) && field.Cells.Where(x => x.IsBomb).All(x => !x.IsOpen))
            {
                field.IsWon = true;
                foreach (var cell in field.Cells.Where(x => x.IsBomb))
                {
                    cell.IsFlag = true;
                }
            }
            _minerFieldRepository.Save(field);
            var fieldViewModel = _mapper.Map<MinerFieldViewModel>(field);

            return View(fieldViewModel);
        }

        public IActionResult Step(long id)
        {
            var cell = _minerCellRepository.Get(id);
            cell.IsOpen = true;
            if (cell.IsBomb)
            {
                cell.FirstOpenedBomb = true;
                cell.Field.IsOver = true;
            }
            if (cell.NearBombsCount==0 && !cell.IsBomb)
            {
                OpenNearWhenBombCountNull(cell);               
            }
            _minerCellRepository.Save(cell);

            return RedirectToAction("Game", new { id = cell.Field.Id });
        }

        public IActionResult SetFlag(long idCell)
        {
            var cell = _minerCellRepository.Get(idCell);
            cell.IsFlag = !(cell.IsFlag);
            _minerCellRepository.Save(cell);
            return Json(true);
        }

        public IActionResult OpenNearWithFlags(long idCell)
        {
            var cell = _minerCellRepository.Get(idCell);
            var countt = _minerFiledBuilder.GetNear(cell.Field.Cells, cell).Where(cell => cell.IsFlag).Count();
            if (cell.NearBombsCount == countt)
            {
                OpenNearWhenBombCountNull(cell);
            }
            return Json(true);
        }

        public IActionResult CheckFlagsAndNearBombsCount(long idCell)
        {
            var cell = _minerCellRepository.Get(idCell);
            var flagsCount = _minerFiledBuilder.GetNear(cell.Field.Cells, cell).Where(cell => cell.IsFlag).Count();
            bool answer;
            if (cell.NearBombsCount == flagsCount)
            {
                answer = true;               
            }
            else
            {
                answer = false;
            }

            return Json(answer);

        }

        public IActionResult GetNearToPress(long idCell)
        {
            var cell = _minerCellRepository.Get(idCell);
            var cellIds = _minerFiledBuilder
                .GetNear(cell.Field.Cells, cell)
                .Where(cell => !cell.IsFlag && !cell.IsOpen)
                .Select(x => x.Id);
            return Json(cellIds);
        }

        private void OpenNearWhenBombCountNull(MinerCell cell)
        {
            var cellsToOpen = _minerFiledBuilder.GetNear(cell.Field.Cells, cell).Where(x => !x.IsOpen && !x.IsFlag && x.Id != cell.Id);

            foreach (var cellToOpen in cellsToOpen)
            {               
                 cellToOpen.IsOpen = true;

                if (cellToOpen.IsBomb)
                {
                    if (!cellToOpen.Field.Cells.Any(cell => cell.FirstOpenedBomb))
                    {
                        cellToOpen.FirstOpenedBomb = true;
                    }
                    cellToOpen.Field.IsOver = true;
                }

                _minerCellRepository.Save(cellToOpen);

                if (cellToOpen.NearBombsCount == 0)
                {
                    OpenNearWhenBombCountNull(cellToOpen);
                }
            }

        }

    }
}
