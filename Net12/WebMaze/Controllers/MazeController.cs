using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net12.Maze;
using Net12.Maze.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.Repositories;
using WebMaze.Models;
using WebMaze.Models.ValidationAttributes;
using WebMaze.Services;

namespace WebMaze.Controllers
{
    public class MazeController : Controller
    {
        private MazeDifficultRepository _mazeDifficultRepository;
        private MazeLevelRepository _mazeLevelRepository;
        private IMapper _mapper;
        private UserService _userService;
        private CellRepository _cellRepository;
        private UserRepository _userRepository;
        public MazeController(MazeDifficultRepository mazzeDifficultRepository, MazeLevelRepository mazeLevelRepository, IMapper mapper, UserService userService, CellRepository cellRepository, UserRepository userRepository = null)
        {
            _mazeDifficultRepository = mazzeDifficultRepository;
            _mapper = mapper;
            _userService = userService;
            _mazeLevelRepository = mazeLevelRepository;
            _cellRepository = cellRepository;
            _userRepository = userRepository;
        }

        //public IActionResult Index(int width, int height)
        //{
        //    var mazeBuilder = new MazeBuilder();
        //    var maze = mazeBuilder.Build(width, height, 50, 100, true);
        //    return View(maze);
        //}
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var ListMaze = _mapper.Map<List<MazeViewModel>>(_userRepository.Get(_userService.GetCurrentUser().Id).ListMazeLevels);
            return View(ListMaze);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Maze(long id)
        {
            if(id == 0)
            {
                return View(_mazeLevelRepository.CreateMaze());
            }

                
            



            return View(_mazeLevelRepository.GetMaze(_userRepository, id));
        }        
        
        [HttpPost]
        [Authorize]
        public IActionResult Maze(int Id)
        {
            var mazeBuilder = new MazeBuilder();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddMazeDifficult(long Id)
        {
            var model = _mapper.Map<MazeDifficultProfileViewModel>(_mazeDifficultRepository.Get(Id));
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddMazeDifficult(MazeDifficultProfileViewModel mazeDifficultProfileViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(mazeDifficultProfileViewModel);
            }

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
