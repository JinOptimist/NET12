using AutoMapper;
using Net12.Maze;
using Net12.Maze.Cells;
using System;
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
        public void ChangeModel(MazeLevelModel model, MazeLevel maze, IMapper mapper)
        {
            var dict = new Dictionary<Type, CellInfo>()
            {
                { typeof(Wall), CellInfo.Wall},
                { typeof(WeakWall), CellInfo.WeakWall},
                { typeof(Ground), CellInfo.Grow},
                { typeof(GoldMine), CellInfo.Goldmine},
                { typeof(Coin), CellInfo.Coin},
                { typeof(Bed),CellInfo.Bed},
                { typeof(Puddle), CellInfo.Puddle},
                { typeof(VitalityPotion), CellInfo.VitalityPotion},
                { typeof(Bless), CellInfo.Bless},
                { typeof(TeleportIn), CellInfo.TeleportIn},
                { typeof(TeleportOut), CellInfo.TeleportOut},
                { typeof(Fountain), CellInfo.Fountain},
                { typeof(Trap), CellInfo.Trap},
                { typeof(HealPotion), CellInfo.HealPotion},
                { typeof(WolfPit), CellInfo.WolfPit},
                { typeof(Tavern), CellInfo.Tavern},
                { typeof(Healer), CellInfo.Healer},

            };

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
                var cellModel = model.Cells.Single(c => c.X == cell.X && c.Y == cell.Y);

                var mod = mapper?.Map<CellModel>(cell);
                if(mod != null) {
                    cellModel.TypeCell = mod.TypeCell;
                    cellModel.Obj1 = mod.Obj1;
                    cellModel.Obj2 = mod.Obj2;
                }
            }
 


        }



    }
}
