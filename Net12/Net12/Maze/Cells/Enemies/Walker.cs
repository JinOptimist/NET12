using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    public class Walker : BaseEnemy
    {

        public Direction _rotation { get; set; } = Direction.Up;
        public Direction _beforeRotation { get; set; } = Direction.Up;
        private int _beforeLeftWallX;
        private int _beforeLeftWallY;

        private int _leftwallX;
        private int _leftwallY;
        public Walker(int x, int y, IMazeLevel maze) : base(x, y, maze)
        {
            _leftwallX = X - 1;
            _leftwallY = Y;
        }

        public override BaseCell BeforeStep()
        {
            //var poses = Maze.Cells.Where(cell => Math.Abs(this.X - cell.X) == 1 && this.Y == cell.Y || Math.Abs(this.Y - cell.Y) == 1 && this.X == cell.X).OfType<Wall>().ToList();
            var no_wall =
                !(_leftwallX < 0
                    || _leftwallX >= Maze.Width
                    || _leftwallY < 0
                    || _leftwallY >= Maze.Height)
                && !(Maze[_leftwallX, _leftwallY] is Wall);

            switch (_rotation)
            {
                case Direction.Up:
                    return RotUp(no_wall);

                case Direction.Down:
                    return RotDown(no_wall);

                case Direction.Left:
                    return RotLeft(no_wall);

                case Direction.Right:
                    return RotRight(no_wall);
            }
            return null;


        }

        public override void AfterStep()
        {
            _leftwallX = _beforeLeftWallX;
            _leftwallY = _beforeLeftWallY;
            _rotation = _beforeRotation;
        }
        private BaseCell RotUp(bool noWall)
        {
            if (!(Maze[X, Y - 1] is Wall) && (Maze[X, Y - 1] != null) && (Maze[_leftwallX, _leftwallY] is Wall || noWall == false))
            {
                _beforeLeftWallX = X - 1;
                _beforeLeftWallY = Y - 1;
                return Maze[X, Y - 1];


            }
            else if (noWall && !(Maze[X - 1, Y] is Wall) && (Maze[X - 1, Y] != null))
            {
                _beforeLeftWallX = X - 1;
                _beforeLeftWallY = Y + 1;
                _beforeRotation = Direction.Left;
                return Maze[X-1, Y];

            }
            else if (noWall == false && !(Maze[X + 1, Y] is Wall) && (Maze[X + 1, Y] != null))
            {
                _beforeLeftWallX = X + 1;
                _beforeLeftWallY = Y - 1;
                _beforeRotation = Direction.Right;
                return Maze[X+1, Y];

            }
            else
            {
                _rotation = Direction.Left;
                _beforeRotation = Direction.Left;
                return BeforeStep();
            }

        }
        private BaseCell RotLeft(bool noWall)
        {


            if (!(Maze[X - 1, Y] is Wall) && (Maze[X - 1, Y] != null) && (Maze[_leftwallX, _leftwallY] is Wall || noWall == false))
            {
                _beforeLeftWallX = X - 1;
                _beforeLeftWallY = Y + 1;
                return Maze[X-1, Y];

            }
            else if (noWall && !(Maze[X, Y + 1] is Wall) && (Maze[X, Y + 1] != null))
            {
                _beforeLeftWallX = X + 1;
                _beforeLeftWallY = Y + 1;
                _beforeRotation = Direction.Down;
                return Maze[X, Y + 1];


            }
            else if (noWall == false && !(Maze[X, Y - 1] is Wall) && (Maze[X, Y - 1] != null))
            {
                _beforeLeftWallX = X - 1;
                _beforeLeftWallY = Y - 1;
                _beforeRotation = Direction.Up;
                return Maze[X, Y - 1];
            }
            else
            {
                _rotation = Direction.Down;
                _beforeRotation = Direction.Down;
                return BeforeStep();
            }

        }
        private BaseCell RotDown(bool noWall)
        {

            if (!(Maze[X, Y + 1] is Wall) && (Maze[X, Y + 1] != null) && (Maze[_leftwallX, _leftwallY] is Wall || noWall == false))
            {
                _beforeLeftWallX = X + 1;
                _beforeLeftWallY = Y + 1;
                return Maze[X, Y + 1];

            }
            else if (noWall && !(Maze[X + 1, Y] is Wall) && (Maze[X + 1, Y] != null))
            {
                _beforeLeftWallX = X + 1;
                _beforeLeftWallY = Y - 1;
                _beforeRotation = Direction.Right;
                return Maze[X+1, Y];

            }
            else if (noWall == false && !(Maze[X - 1, Y] is Wall) && (Maze[X - 1, Y] != null))
            {
                _beforeLeftWallX = X - 1;
                _beforeLeftWallY = Y + 1;
                _beforeRotation = Direction.Left;
                return Maze[X-1, Y];


            }
            else
            {

                _rotation = Direction.Right;
                _beforeRotation = Direction.Right;
                return BeforeStep();
            }

        }
        private BaseCell RotRight(bool noWall)
        {

            if (!(Maze[X + 1, Y] is Wall) && (Maze[X + 1, Y] != null) && (Maze[_leftwallX, _leftwallY] is Wall || noWall == false))
            {
                _beforeLeftWallX = X + 1;
                _beforeLeftWallY = Y - 1;
                return Maze[X+1, Y];


            }
            else if (noWall && !(Maze[X, Y - 1] is Wall) && (Maze[X, Y - 1] != null))
            {
                _beforeLeftWallX = X - 1;
                _beforeLeftWallY = Y - 1;
                _beforeRotation = Direction.Up;
                return Maze[X, Y - 1];

            }
            else if (noWall == false && !(Maze[X, Y + 1] is Wall) && (Maze[X, Y + 1] != null))
            {
                _beforeLeftWallX = X + 1;
                _beforeLeftWallY = Y + 1;
                _beforeRotation = Direction.Up;
                return Maze[X, Y + 1];


            }
            else
            {
                _rotation = Direction.Up;
                _beforeRotation = Direction.Up;
                return BeforeStep();
            }

        }


    }
}
