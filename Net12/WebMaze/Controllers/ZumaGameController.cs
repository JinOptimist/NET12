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
        private IMapper _mapper;

        public ZumaGameController(ZumaGameFieldBuilder zumaGameFieldBuilder, ZumaGameFieldRepository zumaGameFieldRepository, IMapper mapper)
        {
            _zumaGameFieldBuilder = zumaGameFieldBuilder;
            _zumaGameFieldRepository = zumaGameFieldRepository;
            _mapper = mapper;
        }

        public IActionResult StartGame()
        {
            var filed = _zumaGameFieldBuilder.Build(10, 10, 4);

            _zumaGameFieldRepository.Save(filed);

            return RedirectToAction("Game", new { id = filed.Id });
        }
        public IActionResult Game(long id)
        {
            var field = _zumaGameFieldRepository.Get(id);
            var fieldViewModel = _mapper.Map<ZumaGameFieldViewModel>(field);

            return View(fieldViewModel);
        }

    }
}
