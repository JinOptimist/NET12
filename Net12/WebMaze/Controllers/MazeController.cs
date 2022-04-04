using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Net12.Maze;
using Net12.Maze.Cells;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
        private IHubContext<MazeHub> _mazeHub;
        private IHostEnvironment _environment;
        private List<UserMazeActivity> _userMazeActivities = new List<UserMazeActivity>();

        private const int DELAY_MOVING_ENEMIES = 1; // sec
        private const int MAX_TIME_ACTIVITY = 5; // sec

        Mutex mutexObjGoMaze = new Mutex();
        public MazeController(MazeDifficultRepository mazzeDifficultRepository,
            MazeLevelRepository mazeLevelRepository, IMapper mapper,
            UserService userService, CellRepository cellRepository,
            UserRepository userRepository,
            MazeEnemyRepository mazeEnemyRepository,
            PayForActionService payForActionService,
            IHubContext<ChatHub> chatHub,
            IHostEnvironment environment, IHubContext<MazeHub> mazeHub)
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
            _environment = environment;
            _mazeHub = mazeHub;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var MazeList = _userRepository.Get(_userService.GetCurrentUser().Id).ListMazeLevels.Where(m => m.IsActive).ToList();
            var ListMaze = _mapper.Map<List<MazeViewModel>>(MazeList);
            return View(ListMaze);
        }

        public IActionResult Couple()
        {
            return View();
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
            _userMazeActivities.Where(u => u.MazeId == Id).ToList().ForEach(u => u.CancellationTokenSource.Cancel());
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
        public void GoEnemies(long mazeId)
        {
            Console.WriteLine("START MULTHREADING");
            mutexObjGoMaze.WaitOne();
            var dataMaze = (MazeLevelRepository)HttpContext.RequestServices.GetService(typeof(MazeLevelRepository));

            var myModel = dataMaze.Get(mazeId);
            mutexObjGoMaze.ReleaseMutex();
            var mazeActivity = _userMazeActivities.First(u => u.MazeId == mazeId);


            var maze = _mapper.Map<MazeLevel>(myModel);
            maze.GetCoins = GetCoinsFromMaze;

            while ((DateTime.Now - mazeActivity.LastActivity).TotalSeconds < MAX_TIME_ACTIVITY)
            {
                Thread.Sleep(DELAY_MOVING_ENEMIES * 1000);


                Console.WriteLine("GO THREAD ENEMIES");
                maze.EnemiesStep();

                dataMaze.ChangeModel(myModel, maze, _mapper);

                dataMaze.Save(myModel);




            }
            Console.WriteLine("END THREAD");

        }
        [Authorize]
        [HttpGet]
        public IActionResult CreateMaze()
        {
            var listDifficults = _mazeDifficultRepository.GetAll();
            var listViewDifficults = new List<MazeDifficultProfileViewModel>();
            foreach (var complixity in listDifficults)
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



        [Authorize]
        public IActionResult GetMazeData(long mazeId, int stepDirection)
        {
            _userMazeActivities.Where(u => u.MazeId == mazeId).ToList().ForEach(u => u.CancellationTokenSource.Cancel());
            _userMazeActivities.Remove(_userMazeActivities.SingleOrDefault(m => m.MazeId == mazeId));

            var mazeLevelDbModel = _mazeLevelRepository.Get(mazeId);
            var maze = _mapper.Map<MazeLevel>(mazeLevelDbModel);
            maze.GetCoins = GetCoinsFromMaze;

            switch (stepDirection)
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

            _mazeLevelRepository.ChangeModel(mazeLevelDbModel, maze, _mapper);

            _mazeLevelRepository.Save(mazeLevelDbModel);

            var viewModel = _mapper.Map<MazeLevelViewModel>(mazeLevelDbModel);

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            var mazeActivity = new UserMazeActivity() { MazeId = maze.Id, CancellationTokenSource = cancelTokenSource, LastActivity = DateTime.Now };
            _userMazeActivities.Add(mazeActivity);

            //Task t_Enemies = new Task(() => GoEnemies(mazeId), token);
            //t_Enemies.Start();
            //var s = HttpContext.User.Identity.Name;
            //do
            //{
            //    mutexObjGoMaze.WaitOne();
            //    var mazeLevelDbModel_cycle = _mazeLevelRepository.Get(mazeId);
            //    mutexObjGoMaze.ReleaseMutex();

            //    var viewModel_cycle = _mapper.Map<MazeLevelViewModel>(mazeLevelDbModel);
            //    yield return Json(viewModel_cycle);
            //    _mazeHub.Clients.User(s).SendAsync("ChangingMazeCells", viewModel_cycle);
            //    Thread.Sleep(1 * DELAY_MOVING_ENEMIES);
            //} while (_userMazeActivities.Where(u => u.MazeId == mazeId).Any() || !t_Enemies.IsCompleted || !t_Enemies.IsCanceled);
            //_userMazeActivities.Where(u => u.MazeId == mazeId).ToList().ForEach(u => u.CancellationTokenSource.Cancel());

            //yield break;
            var threadContext = HttpContext;

            var result = Task.Run(() =>
           {
               var s = HttpContext.User.Identity.Name;
               var dataMaze = (MazeLevelRepository)HttpContext.RequestServices.GetService(typeof(MazeLevelRepository));

               var myModel = dataMaze.Get(mazeId);
               mutexObjGoMaze.ReleaseMutex();
               var mazeActivity = _userMazeActivities.First(u => u.MazeId == mazeId);


               var maze = _mapper.Map<MazeLevel>(myModel);
               maze.GetCoins = GetCoinsFromMaze;

               while ((DateTime.Now - mazeActivity.LastActivity).TotalSeconds < MAX_TIME_ACTIVITY)
               {
                   Thread.Sleep(DELAY_MOVING_ENEMIES * 1000);


                   Console.WriteLine("GO THREAD ENEMIES");
                   maze.EnemiesStep();

                   dataMaze.ChangeModel(myModel, maze, _mapper);

                   dataMaze.Save(myModel);
               }
               var mazeLevelDbModel_cycle = _mazeLevelRepository.Get(mazeId);

               var viewModel_cycle = _mapper.Map<MazeLevelViewModel>(mazeLevelDbModel);
               _mazeHub.Clients.User(s).SendAsync("ChangingMazeCells", viewModel_cycle);
               Thread.Sleep(1 * DELAY_MOVING_ENEMIES);

           });
           



        }
        public IActionResult Wonderful(long difficultId)
        {
            var diffcult = _mazeDifficultRepository.Get(difficultId);
            _payForActionService.CreatorEarnMoney(diffcult.Creater.Id, 10);

            return RedirectToAction("ManageMazeDifficult", "Maze");
        }

        public IActionResult Awful(long difficultId)
        {
            var diffcult = _mazeDifficultRepository.Get(difficultId);

            _payForActionService.CreatorDislikeFine(diffcult.Creater.Id, TypesOfPayment.Fine);

            return RedirectToAction("ManageMazeDifficult", "Maze");
        }

        public IActionResult GetUrlsForCouple()
        {
            var path = Path.Combine(_environment.ContentRootPath, "wwwroot", "imgYellowTeam");
            var urls =
                Directory.GetFiles(path)
                    .Select(x => Path.GetFileNameWithoutExtension(x))
                    .Where(x => x != "fon" && x != "stoc")
                    .Select(x => $"/imgYellowTeam/{x}.jpg");

            return Json(urls);
        }

        public IActionResult CoupleWin(int CardsCount, int Steps)
        {

            return Json(true);
        }

        public void GetCoinsFromMaze(int coins)
        {
            var user = _userService.GetCurrentUser();
            user.Coins += coins;
            _userRepository.Save(user);
        }
    }
}
