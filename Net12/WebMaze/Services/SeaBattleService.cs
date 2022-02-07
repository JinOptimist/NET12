using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.SeaBattle;
using WebMaze.EfStuff.Repositories.SeaBattle;

namespace WebMaze.Services
{
    public class SeaBattleService
    {
        private UserService _userService;
        private SeaBattleCellRepository _seaBattleCellRepository;
        private Random _random;

        public SeaBattleService(UserService userService, SeaBattleCellRepository seaBattleCellRepository)
        {
            _userService = userService;
            _seaBattleCellRepository = seaBattleCellRepository;
        }
        public enum ShipSize
        {
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4
        }

        public SeaBattleField BuildField(SeaBattleDifficult difficult)
        {
            var field = new SeaBattleField()
            {
                Width = difficult.Width,
                Height = difficult.Height,
                Cells = new List<SeaBattleCell>(),
                Gamer = _userService.GetCurrentUser(),
                IsActive = true
            };

            for (int y = 0; y < difficult.Height; y++)
            {
                for (int x = 0; x < difficult.Width; x++)
                {
                    var myCell = new SeaBattleCell()
                    {
                        X = x,
                        Y = y,
                        ShipLength = 0,
                        SideField = false,
                        ShipHere = false,
                        IsActive = true
                    };
                    field.Cells.Add(myCell);

                    var enemyCell = new SeaBattleCell()
                    {
                        X = x,
                        Y = y,
                        ShipLength = 0,
                        SideField = true,
                        ShipHere = false,
                        IsActive = true
                    };
                    field.Cells.Add(enemyCell);
                }
            }

            var fourSizeShipCount = difficult.FourSizeShip;
            while (fourSizeShipCount != 0)
            {
                var height = _random.Next(difficult.Height);
                var width = _random.Next(difficult.Width);


                // 0 - left 1 - right 2 - up 3 - down
                var direction = _random.Next(4);

                switch (direction)
                {
                    case 0:

                        break;
                    case 1:

                        break;
                    case 2:
                        if (height - (int)ShipSize.Four >= 0)
                            if (!field.Cells
                                .Where(x => x.X == width && Math.Abs(x.Y - height) <= 4 && x.ShipHere)
                                .ToList()
                                .Any())
                            {
                                for (int i = 0; i < (int)ShipSize.Four; i++)
                                {
                                    var cell = field.Cells
                                        .Where(x => x.X == width && x.Y == height - i)
                                        .FirstOrDefault();
                                    cell.ShipLength = (int)ShipSize.Four;
                                    cell.ShipHere = true;
                                    //_seaBattleCellRepository.Save(cell);

                                    field.Cells
                                        .Where(x => x.X == width && Math.Abs(x.Y - height) <= 4)
                                        .ToList()
                                        .ForEach(x=> { 
                                            x.ShipLength = (int)ShipSize.Four; 
                                            x.ShipHere = true; 
                                        });
                                        


                                }
                            }

                        break;
                    case 3:

                        break;
                    default:
                        break;
                }



            }




            return field;
        }



    }
}
