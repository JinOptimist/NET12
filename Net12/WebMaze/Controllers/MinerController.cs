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

        public IActionResult Step(long idcell)
        {
            var cell = _minerCellRepository.Get(idcell);
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

            var field = _minerFieldRepository.Get(cell.Field.Id);
            if (field.IsOver)
            {
                foreach (var cel in field.Cells.Where(x => x.IsBomb && !x.IsFlag))
                {
                    cel.IsOpen = true;
                }
            }
            if (field.Cells.Where(x => !x.IsBomb).All(x => x.IsOpen) && field.Cells.Where(x => x.IsBomb).All(x => !x.IsOpen))
            {
                field.IsWon = true;
                foreach (var cel in field.Cells.Where(x => x.IsBomb))
                {
                    cel.IsFlag = true;
                }
            }
            _minerFieldRepository.Save(field);

            var fieldViewModel = _mapper.Map<MinerFieldViewModel>(field);

            return Json(fieldViewModel);
        }

        public IActionResult SetFlag(long idCell)
        {
            var cell = _minerCellRepository.Get(idCell);
            cell.IsFlag = !(cell.IsFlag);
            _minerCellRepository.Save(cell);
            var isFlag = cell.IsFlag;
            return Json(isFlag);
        }

        public IActionResult CheckStartTheGame(long idCell)
        {
            var cell = _minerCellRepository.Get(idCell);
            if (!(cell.Field.IsOver || cell.Field.IsWon))
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        public IActionResult OpenNearWithFlags(long idCell)
        {
            var cell = _minerCellRepository.Get(idCell);
            var countt = _minerFiledBuilder.GetNear(cell.Field.Cells, cell).Where(cell => cell.IsFlag).Count();
            if (cell.NearBombsCount == countt)
            {
                OpenNearWhenBombCountNull(cell);
            }

            var field = _minerFieldRepository.Get(cell.Field.Id);
            var fieldViewModel = _mapper.Map<MinerFieldViewModel>(field);

            return Json(fieldViewModel);
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

        public IActionResult CheckFlagsAmount(long idCell)
        {
            var cell = _minerCellRepository.Get(idCell);
            var flagsAmount = 10 - cell.Field.Cells.Where(x => x.IsFlag).Count();

            return Json(flagsAmount);
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
                    //cell.Field.IsPlayingNow = false;
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
