using AutoMapper;
using Net12.Maze;
using Net12.Maze.Cells;
using System.Collections.Generic;
using System.Linq;
using WebMaze.EfStuff.DbModel;
using WebMaze.Models;
using WebMaze.Services;

namespace WebMaze.EfStuff.Repositories
{
    public class MazeLevelRepository : BaseRepository<MazeLevelModel>
    {
        private UserService _userService;
        private CellRepository _cellRepository;
        private IMapper _mapper;
        public MazeLevelRepository(WebContext webContext, UserService userService, CellRepository cellRepository, IMapper mapper) : base(webContext)
        {
            _userService = userService;
            _cellRepository = cellRepository;
            _mapper = mapper;
        }

        public MazeLevelViewId GetMaze(UserRepository _userRepository, long id)
        {

            MazeLevel Mazelevel = new MazeLevel();
            var cellList = new List<BaseCell>();
            var mazelist = _userRepository.Get(_userService.GetCurrentUser().Id)?.ListMazeLevels;
            if (mazelist.Any())
            {
                var mazeModel = mazelist.SingleOrDefault(maze => maze.Id == id);
                if(mazeModel == null)
                {
                    return null;
                }
                foreach (var cell in mazeModel.Cells)
                {
                    cellList.Add(_cellRepository.GetCurrentCell(cell, Mazelevel));

                }
                Mazelevel.Cells = cellList;
                Mazelevel.Width = mazeModel.Width;
                Mazelevel.Height = mazeModel.Height;
                Mazelevel.Hero = new Hero(mazeModel.HeroX, mazeModel.HeroY, Mazelevel, mazeModel.HeroNowHp, mazeModel.HeroMaxHp);
                var mazeId = new MazeLevelViewId(mazeModel.Id, Mazelevel);
                return mazeId;
            }
                return null;
        }
        public MazeLevelViewId CreateMaze()
        {
            var Mazelevel = new MazeBuilder().Build(10, 10, 10, 100, true);
            var mazeId = new MazeLevelViewId(0, Mazelevel);
            SaveMaze(mazeId);
            return (mazeId);
        }

        public void SaveMaze(MazeLevelViewId Mazelevel)
        {
            var MazeModel = Get(Mazelevel.Id);
            if(MazeModel == null)
            {
                MazeModel = new MazeLevelModel
                {
                    IsActive = true,
                    Creator = _userService.GetCurrentUser()
                };
            }
            MazeModel.Cells = new List<CellModel>();
            foreach (var cell in Mazelevel.MazeLevel.Cells)
            {
                MazeModel.Cells.Add(_cellRepository.GetCurrentCell(cell, MazeModel));
            }
            MazeModel.Height = Mazelevel.MazeLevel.Height;
            MazeModel.Width = Mazelevel.MazeLevel.Width;
            MazeModel.HeroMaxFatigure = Mazelevel.MazeLevel.Hero.MaxFatigue;
            MazeModel.HeroMaxHp = Mazelevel.MazeLevel.Hero.Max_hp;
            MazeModel.HeroNowHp = Mazelevel.MazeLevel.Hero.Hp;
            MazeModel.HeroX = Mazelevel.MazeLevel.Hero.X;
            MazeModel.HeroY = Mazelevel.MazeLevel.Hero.Y;
            MazeModel.Id = Mazelevel.Id;
            Save(MazeModel);

        }
    }
}
