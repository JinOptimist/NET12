using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
using WebMaze.Models.Enums;
using WebMaze.Models.ValidationAttributes;
using WebMaze.Services;
using WebMaze.SignalRHubs;

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
        private readonly PayForActionService _payForActionService;
        private IHubContext<ChatHub> _chatHub;
        public MazeController(MazeDifficultRepository mazzeDifficultRepository, 
            MazeLevelRepository mazeLevelRepository, IMapper mapper, 
            UserService userService, CellRepository cellRepository, 
            UserRepository userRepository, 
            MazeEnemyRepository mazeEnemyRepository, 
            PayForActionService payForActionService, 
            IHubContext<ChatHub> chatHub)
        {
            _mazeDifficultRepository = mazzeDifficultRepository;
            _mapper = mapper;
            _userService = userService;
            _mazeLevelRepository = mazeLevelRepository;
            _cellRepository = cellRepository;
            _userRepository = userRepository;
            _mazeEnemyRepository = mazeEnemyRepository;
            _payForActionService = payForActionService;
            _chatHub = chatHub;
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


            _chatHub.Clients.All.SendAsync("StartMaze", _userService.GetCurrentUser().Name);

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
            var listDifficults = _mazeDifficultRepository.GetAll();
            var listViewDifficults = new List<MazeDifficultProfileViewModel>();
            foreach(var complixity in listDifficults)
            {
                listViewDifficults.Add(_mapper.Map<MazeDifficultProfileViewModel>(complixity));
            }
            return View(listViewDifficults);
        }
        [Authorize]
        [HttpPost]
        public IActionResult CreateMaze(MazeDifficultProfileViewModel viewMaze)
        {
            var complixity = _mazeDifficultRepository.Get(viewMaze.Id);
            if (complixity is null || _userService.GetCurrentUser().Coins <= complixity.CoinCount)
            {
                return RedirectToAction("Index");
            }
            else
            {
                _userService.GetCurrentUser().Coins -= complixity.CoinCount;

                var maze = new MazeBuilder().Build(complixity.Width, complixity.Height, complixity.HeroMaxHp, complixity.HeroMaxHp, GetCoinsFromMaze, false);
                maze.Hero.MaxFatigue = complixity.HeroMaxFatigue;

                var model = _mapper.Map<MazeLevelWeb>(maze);
                model.IsActive = true;
                model.Name = viewMaze.Name;
                model.Creator = _userRepository.Get(_userService.GetCurrentUser().Id);
                _mazeLevelRepository.Save(model);

                _chatHub.Clients.All.SendAsync("BuyMaze", _userService.GetCurrentUser().Name, complixity.Name, complixity.CoinCount);
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
        [PayForAddActionFilter(TypesOfPayment.Small)]
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

        [IsAdmin]
        public IActionResult ManageMazeDifficult()
        {
            var mazeDifficultProfileViewModels = new List<MazeDifficultProfileViewModel>();

            mazeDifficultProfileViewModels = _mazeDifficultRepository.GetAll()
                .Select(x => _mapper.Map<MazeDifficultProfileViewModel>(x)).ToList();

            return View(mazeDifficultProfileViewModels);
        }

        [IsAdmin]
        public IActionResult RemoveMazeDifficult(long Id)
        {
            _mazeDifficultRepository.Remove(Id);
            return RedirectToAction("ManageMazeDifficult", "Maze");
        }

        public IActionResult Wonderful(long difficultId)
        {
            var diffcult = _mazeDifficultRepository.Get(difficultId);
            _payForActionService.CreatorEarnMoney(diffcult.Creater.Id, 10);

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
