using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    public class BullEnemy : BaseEnemy
    {
        private Random _random = new Random();

        private Direction _heroDirection;
        public BullEnemy(int x, int y, MazeLevel maze) : base(x, y, maze) 
        {
            _heroDirection = GetRandomDirection();
        }

        public override void Step()
        {
            DetectedHero();
            if (CanBeStepByDirection(_heroDirection))
            {
                StepByDirection(_heroDirection);
                return;
            }

            var directions = (Direction[])Enum.GetValues(typeof(Direction));
            directions = directions.Where(x => x != _heroDirection).ToArray();

            do
            {
                var direction = GetRandomDirection(directions);
                if(CanBeStepByDirection(direction))
                {
                    StepByDirection(direction);
                    _heroDirection = direction;
                    DetectedHero();
                    return;
                }

                directions = directions.Where(x => x != direction).ToArray();
            }
            while (directions.Length > 0);
        }

        private Direction GetRandomDirection(Direction[] listOfDirections = null)
        {
            var directions = listOfDirections ?? (Direction[])Enum.GetValues(typeof(Direction));
            var index = _random.Next(directions.Length);
            return directions[index];
        }

        private bool CanBeStepByDirection(Direction direction)
        {
            var cell = GetCellByDirection(direction);

            return cell != null && !(cell is Wall);
        }

        private BaseCell GetCellByDirection(Direction direction)
        {
            var x = X + (direction == Direction.Up || direction == Direction.Down ? 0 : direction == Direction.Right ? 1 : -1);
            var y = Y + (direction == Direction.Left || direction == Direction.Right ? 0 : direction == Direction.Up ? 1 : -1);
            if(x<0 || x== Maze.Width || y<0 || y==Maze.Height)
            {
                return null;
            }

            return Maze[x, y];
        }

        private void StepByDirection(Direction direction)
        {
            var cell = GetCellByDirection(direction);
            X = cell.X;
            Y = cell.Y;
        }

        private void DetectedHero()
        {
            if (Maze.Hero.X == X && Maze.Hero.Y == Y)
            {
                Maze.Hero.Hp--;
            }
        }

        public override bool TryToStep()
        {
            return true;
        }
    }
}
