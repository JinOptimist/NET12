using Net12.Maze.Cells;
using Net12.Maze.Cells.Enemies;
using Net12.Maze.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12.Maze
{

    public class MazeLevel : IMazeLevel

    {
        public long Id { get; set; }

        public List<BaseCell> Cells { get; set; } = new List<BaseCell>();

        public List<BaseEnemy> Enemies { get; set; } = new List<BaseEnemy>();


        public int Width { get; set; }
        public int Height { get; set; }

        public IHero Hero { get; set; }

        public bool ExitIsOpen { get; set; }
        public string Message { get; set; } = "Play";
        public int CoinsToOpenTheDoor { get; set; }
        public MazeStatusEnum MazeStatus { get; set; }
        public Action<int> GetCoins { get; set; }

        public IBaseCell GetCellOrUnit(int x, int y)
        {
            if (Hero.X == x && Hero.Y == y)
            {
                return Hero;
            }
            var enemy = Enemies.SingleOrDefault(enemy => enemy.X == x && enemy.Y == y);
            if (enemy != null)
            {
                return enemy;
            }

            return this[x, y];
        }

        public BaseCell this[int x, int y]
        {
            get
            {
                return Cells.SingleOrDefault(cell => cell.X == x && cell.Y == y);
            }

            set
            {
                var oldCell = this[x, y];
                if (oldCell != null)
                {
                    Cells.Remove(oldCell);
                }

                Cells.Add(value);
            }
        }

        public void ReplaceCell(BaseCell baseCell)
        {
            var oldCell = this[baseCell.X, baseCell.Y];
            if (oldCell != null)
            {
                Cells.Remove(oldCell);
            }

            Cells.Add(baseCell);
        }

        public void HeroStep(Direction direction)
        {

            Message = "Step";
            var heroPositionX = Hero.X;
            var heroPositionY = Hero.Y;

            switch (direction)
            {
                case Direction.Up:
                    heroPositionY--;
                    break;
                case Direction.Right:
                    heroPositionX++;
                    break;
                case Direction.Down:
                    heroPositionY++;
                    break;
                case Direction.Left:
                    heroPositionX--;
                    break;
                case Direction.Spacebar:
                    if (Hero.CurrentFatigue < Hero.MaxFatigue)
                    {
                        Hero.CurrentFatigue++;
                    }
                    break;
                default:
                    break;

            }


            if (Hero.CurrentFatigue < Hero.MaxFatigue && Hero.Hp > 0)
            {
                Hero.CurrentFatigue++;
            }
            else
            {
                MazeStatus = MazeStatusEnum.Wasted;
                Message = "WASTED";
                return;
            }

            var cellToStep = GetCellOrUnit(heroPositionX, heroPositionY);

            if (cellToStep?.TryToStep() ?? false)
            {
                Hero.X = heroPositionX;
                Hero.Y = heroPositionY;
                ExitIsOpen = Hero.Money >= CoinsToOpenTheDoor;
            }

            Enemies.ForEach(x => x.GoStep());

            //foreach (var enemy in Enemies)
            //{
            //    enemy.Step();
            //}
        }
        public void EnemiesStep()
        {
            Enemies.ForEach(x => x.GoStep());
        }
    }
}
