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
        public MazeLevelRepository(WebContext webContext, UserService userService, CellRepository cellRepository) : base(webContext)
        {
            _userService = userService;
            _cellRepository = cellRepository;
        }

        public MazeLevelViewId GetMaze(UserRepository _userRepository, long id)
        {
            if(_userRepository is null)
            {
                return null;
            }
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
            SaveMaze(Mazelevel);
            var mazeId = new MazeLevelViewId(0, Mazelevel);
            return (mazeId);
        }

        public void SaveMaze(MazeLevel Mazelevel)
        {
            var MazeModel = new MazeLevelModel();

            MazeModel.IsActive = true;
            MazeModel.Creator = _userService.GetCurrentUser();
            MazeModel.Cells = new List<CellModel>();
            foreach (var cell in Mazelevel.Cells)
            {
                MazeModel.Cells.Add(_cellRepository.GetCurrentCell(cell, MazeModel));
            }
            MazeModel.Height = Mazelevel.Height;
            MazeModel.Width = Mazelevel.Width;
            MazeModel.HeroMaxFatigure = Mazelevel.Hero.MaxFatigue;
            MazeModel.HeroMaxHp = Mazelevel.Hero.Max_hp;
            MazeModel.HeroNowHp = Mazelevel.Hero.Hp;
            MazeModel.HeroX = Mazelevel.Hero.X;
            MazeModel.HeroY = Mazelevel.Hero.Y;
            Save(MazeModel);

        }
    }
}
