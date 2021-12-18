using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net12.Maze;
using Net12.Maze.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Controllers.AuthAttribute;
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
        private MazeEnemyRepository _mazeEnemyRepository;
        public MazeController(MazeDifficultRepository mazzeDifficultRepository, MazeLevelRepository mazeLevelRepository, IMapper mapper, UserService userService, CellRepository cellRepository, UserRepository userRepository = null, MazeEnemyRepository mazeEnemyRepository = null)
        {
            _mazeDifficultRepository = mazzeDifficultRepository;
            _mapper = mapper;
            _userService = userService;
            _mazeLevelRepository = mazeLevelRepository;
            _cellRepository = cellRepository;
            _userRepository = userRepository;
            _mazeEnemyRepository = mazeEnemyRepository;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var MazeList = _userRepository.Get(_userService.GetCurrentUser().Id).ListMazeLevels.Where(m => m.IsActive).ToList();
            var ListMaze = _mapper.Map<List<MazeViewModel>>(MazeList);
            return View(ListMaze);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Maze(long id)
        {
            if (!_userRepository.Get(_userService.GetCurrentUser().Id).ListMazeLevels.Any(maze => maze.Id == id))
            {
                return RedirectToAction("Index");
            }
            var Model = _mazeLevelRepository.Get(id);
            var maz = _mapper.Map<MazeLevel>(Model);
            if (maz is null || !Model.IsActive)
            {
                return RedirectToAction("Index");
            }




            return View(maz);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Maze(long Id, int turn)
        {
            var myModel = _mazeLevelRepository.Get(Id);
            var maze = _mapper.Map<MazeLevel>(myModel);
            maze.GetCoins = GetCoinsFromMaze;

            switch (turn)
            {
                case 1:
                    maze.HeroStep(Direction.Up);
                    break;
                case 2:
                    maze.HeroStep(Direction.Down);
                    break;
                case 3:
                    maze.HeroStep(Direction.Left);
                    break;
                case 4:
                    maze.HeroStep(Direction.Right);
                    break;
            }

            _mazeLevelRepository.ChangeModel(myModel, maze, _mapper);

            _mazeLevelRepository.Save(myModel);
            return View(maze);
        }
        [Authorize]
        [HttpGet]
        public IActionResult CreateMaze()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult CreateMaze(MazeViewModel nullMaze)
        {   //TODO: COST OF LEVELS
            if (_userService.GetCurrentUser().Coins < 100)
            {
                return RedirectToAction("Index");
            }
            else
            {
                _userService.GetCurrentUser().Coins -= 100;
                //TODO: CHOOSE DIFFICULITY
                //  var maze = new MazeBuilder().Build(10, 10, 100, 100, true);
                var maze = new MazeBuilder().Build(10, 10, 100, 100, GetCoinsFromMaze, false);
                var model = _mapper.Map<MazeLevelWeb>(maze);
                model.IsActive = true;

                //TODO: CREATE ABILITY CHOOSE YOUR NAME LEVEL
                model.Name = nullMaze.Name;
                model.Creator = _userRepository.Get(_userService.GetCurrentUser().Id);

                _mazeLevelRepository.Save(model);


                return RedirectToAction("Index");
            }
        }

        [Authorize]
        public IActionResult DeleteMaze(long Id)
        {
            var user = _userRepository.Get(_userService.GetCurrentUser().Id);
            if (user.ListMazeLevels.Any(m => m.Id == Id))
            {
                var maze = _mazeLevelRepository.Get(Id);

                _cellRepository.Remove(maze.Cells);
                _mazeEnemyRepository.Remove(maze.Enemies);
                _mazeLevelRepository.Remove(Id);

            }

            return RedirectToAction("Index");
        }

        [Authorize]
        [IsAdmin]
        [HttpGet]
        public IActionResult AddMazeDifficult(long Id)
        {
            var model = _mapper.Map<MazeDifficultProfileViewModel>(_mazeDifficultRepository.Get(Id));
            return View(model);
        }

        [Authorize]
        [IsAdmin]
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

        public void GetCoinsFromMaze(int coins)
        {
            var user = _userService.GetCurrentUser();
            user.Coins += coins;
            _userRepository.Save(user);
        }
    }
}
