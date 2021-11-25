using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net12.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class MazeController : Controller
    {
        private MazeDifficultRepository _mazeDifficultRepository;
        private IMapper _mapper;
        private UserService _userService;

        public MazeController(MazeDifficultRepository mazzeDifficultRepository, IMapper mapper, UserService userService)
        {
            _mazeDifficultRepository = mazzeDifficultRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public IActionResult Index(int width, int height)
        {
            var mazeBuilder = new MazeBuilder();
            var maze = mazeBuilder.Build(width, height, 50, 100, true);
            return View(maze);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddMazeDifficult()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddMazeDifficult(MazeDifficultProfileViewModel mazeDifficultProfileViewModel)
        {
            var dbMazeDifficult = _mapper.Map<MazeDifficultProfile>(mazeDifficultProfileViewModel);
            dbMazeDifficult.IsActive = true;
            dbMazeDifficult.Creater = _userService.GetCurrentUser();

            _mazeDifficultRepository.Save(dbMazeDifficult);

            return RedirectToAction("ManageMazeDifficult", "Maze");
        }

        public IActionResult ManageMazeDifficult()
        {
            var mazeDifficultProfileViewModels = new List<MazeDifficultProfileViewModel>();

            mazeDifficultProfileViewModels = _mazeDifficultRepository.GetAll()
                .Select(x => _mapper.Map<MazeDifficultProfileViewModel>(x)).ToList();

            return View(mazeDifficultProfileViewModels);
        }

        public IActionResult RemoveMazeDifficult(long Id)
        {
            _mazeDifficultRepository.Remove(Id);
            return RedirectToAction("ManageMazeDifficult", "Maze");
        }
    }
}
