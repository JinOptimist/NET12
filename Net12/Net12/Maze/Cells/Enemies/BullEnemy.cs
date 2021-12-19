using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    public class BullEnemy : BaseEnemy
    {
        private Random _random = new Random();
        private Direction changeDirection;
        public Direction _heroDirection;
        public BullEnemy(int x, int y, MazeLevel maze) : base(x, y, maze) 
        {
            _heroDirection = GetRandomDirection();
        }

        public override BaseCell BeforeStep()
        {
            DetectedHero();
            if (CanBeStepByDirection(_heroDirection))
            {
                return GetCellByDirection(_heroDirection);
             
            }

            var AllPossibleDirections = (Direction[])Enum.GetValues(typeof(Direction));
            AllPossibleDirections = AllPossibleDirections.Where(x => x != _heroDirection).ToArray();

            do
            {
                var direction = GetRandomDirection(AllPossibleDirections);
                if(CanBeStepByDirection(direction))
                {
                    changeDirection = direction;
                    return GetCellByDirection(direction);

                }

                AllPossibleDirections = AllPossibleDirections.Where(x => x != direction).ToArray();
            }
            while (AllPossibleDirections.Length > 0);
            return null;
        }
        public override void AfterStep()
        {
            DetectedHero();
            _heroDirection = changeDirection;
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
            //null isn't a wall so if we had null cell, return would be true instead false
            return cell != null && !(cell is Wall);
        }

        private BaseCell GetCellByDirection(Direction direction)
        {            
            var newLocationX = X;
            var newLocationY = Y;
            switch (direction)
            {
                case Direction.Up:
                    newLocationY--;
                    break;
                case Direction.Right:
                    newLocationX++;
                    break;
                case Direction.Down:
                    newLocationY++;
                    break;
                case Direction.Left:
                    newLocationX--;
                    break;
                default:
                    break;
            }

            return Maze[newLocationX, newLocationY];
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
