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
        public void ChangeModel(MazeLevelModel model, MazeLevel maze)
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
                var CellModel = model.Cells.Single(c => c.X == cell.X && c.Y == cell.Y);
                
                if(cell is VitalityPotion)
                {
                    VitalityPotion vit = (VitalityPotion)cell;
                    CellModel.Obj1 = vit.AddMaxFatigue;
                    CellModel.TypeCell = CellInfo.VitalityPotion;

                } else if(cell is Coin)
                {
                    Coin vit = (Coin)cell;
                    CellModel.Obj1 = vit.CoinCount;
                    CellModel.TypeCell = CellInfo.Coin;
                }
                else if (cell is WeakWall)
                {
                    WeakWall vit = (WeakWall)cell;
                    CellModel.Obj1 = vit._vitalityOfWeakWall;
                    CellModel.TypeCell = CellInfo.WeakWall;
                }
                else if (cell is GoldMine)
                {
                    GoldMine vit = (GoldMine)cell;
                    CellModel.Obj1 = vit.currentGoldMineMp;
                    CellModel.TypeCell = CellInfo.Goldmine;
                }
                else
                {
                    CellModel.TypeCell = dict[cell.GetType()];
                }

            }
 


        }



    }
}
