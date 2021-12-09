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
            var fieldViewModel = _mapper.Map<MinerFieldViewModel>(field);
            return View(fieldViewModel);
        }

        public IActionResult Step(long id)
        {
            var cell = _minerCellRepository.Get(id);
            cell.IsOpen = true;
            _minerCellRepository.Save(cell);

            return RedirectToAction("Game", new { id = cell.Field.Id });
        }
    }
}
