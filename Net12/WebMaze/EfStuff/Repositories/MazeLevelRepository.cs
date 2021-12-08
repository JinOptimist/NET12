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
        public MazeLevelRepository(WebContext webContext) : base(webContext)
        {

        }
        public void ChangeModel(MazeLevelModel model, MazeLevel maze)
        {

            model.Height = maze.Height;
            model.Width = maze.Width;
            model.HeroMaxFatigure = maze.Hero.MaxFatigue;
            model.HeroMaxHp = maze.Hero.Max_hp;
            model.HeroNowFatigure = maze.Hero.CurrentFatigue;
            model.HeroNowHp = maze.Hero.Hp;
            model.HeroX = maze.Hero.X;
            model.HeroY = maze.Hero.Y;

            foreach (var cell in maze.Cells)
            {
                if (cell is Ground)
                {
                    model.Cells.Where(c => { c.TypeCell = CellInfo.Grow; return true; }); 
                }
                else if (cell is Wall)
                {
                    model.Cells.Where(c => { c.TypeCell = CellInfo.Wall; return true; });
                }
            }

        }



    }
}
