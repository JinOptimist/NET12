using Net12.Maze.Cells;
using Net12.Maze.Cells.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12.Maze
{
    public class MazeLevel
    {
        public List<BaseCell> Cells { get; set; } = new List<BaseCell>();

        public List<BaseEnemy> Enemies { get; set; } = new List<BaseEnemy>();

        public int Width { get; set; }
        public int Height { get; set; }
        public Hero Hero { get; set; }

        public string Message { get; set; } = "";

        public BaseCell GetCellOrUnit(int x, int y)
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

        public void HeroStep(Direction direction)
        {
            Message = "";
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
                default:
                    break;
            }

            var cellToStep = this[heroPositionX, heroPositionY];

            if (cellToStep?.TryToStep() ?? false)
            {
                Hero.X = heroPositionX;
                Hero.Y = heroPositionY;
            }

            Enemies.ForEach(x => x.Step());

            //foreach (var enemy in Enemies)
            //{
            //    enemy.Step();
            //}
        }
    }
}
