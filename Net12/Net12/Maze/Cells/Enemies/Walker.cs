using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    class Walker : BaseEnemy
    {
  
        Direction Rot { get; set; }
        private int _leftwallX;
        private int _leftwallY;
        public Walker(int x, int y, IMazeLevel maze) : base(x, y, maze)
        {
            Rot = Direction.Up;
            _leftwallX = X - 1;
            _leftwallY = Y;

        }

        public override void Step()
        {
            //var poses = Maze.Cells.Where(cell => Math.Abs(this.X - cell.X) == 1 && this.Y == cell.Y || Math.Abs(this.Y - cell.Y) == 1 && this.X == cell.X).OfType<Wall>().ToList();
            var no_wall =
                !(_leftwallX < 0
                    || _leftwallX >= Maze.Width
                    || _leftwallY < 0
                    || _leftwallY >= Maze.Height)
                && !(Maze[_leftwallX, _leftwallY] is Wall);

            switch (Rot)
            {
                case Direction.Up:
                    RotUp(no_wall);
                    break;
                     
                case Direction.Down:
                    _rotDown(no_wall);
                    break;
                
                case Direction.Left:
                    _rotLeft(no_wall);
                    break;
                
                case Direction.Right:
                    _rotRight(no_wall);
                    break;
            }

            hit();
        }

        private void hit()
        {
            if (X == Maze.Hero.X && Y == Maze.Hero.Y)
            {
                Maze.Hero.Hp--;
            }
        }
        private void RotUp(bool noWall)
        {
                if (Maze[X, Y - 1] is Ground && (Maze[_leftwallX, _leftwallY] is Wall || noWall == false))
                {
                    Y--;
                    _leftwallX = X - 1;
                    _leftwallY = Y;

                }
                else if (noWall && Maze[X - 1, Y] is Ground)
                {

                    X--;
                    _leftwallX = X;
                    _leftwallY = Y + 1;
                    Rot = Direction.Left;

                }
                else if (noWall == false && Maze[X + 1, Y] is Ground)
                {

                    X++;
                    _leftwallX = X;
                    _leftwallY = Y - 1;
                    Rot = Direction.Right;

                }
                else
                {
                    Rot = Direction.Left;
                    Step();
                }
            
        }
        private void _rotLeft(bool no_wall)
        {
              
            
                if (Maze[X - 1, Y] is Ground && (Maze[_leftwallX, _leftwallY] is Wall || no_wall == false))
                {
                    X--;
                    _leftwallX = X;
                    _leftwallY = Y + 1;

                }
                else if (no_wall && Maze[X, Y + 1] is Ground)
                {

                    Y++;
                    _leftwallX = X + 1;
                    _leftwallY = Y;
                    Rot = Direction.Down;

                }
                else if (no_wall == false && Maze[X, Y - 1] is Ground)
                {

                    Y--;
                    _leftwallX = X - 1;
                    _leftwallY = Y;
                    Rot = Direction.Up;

                }
                else
                {
                    Rot = Direction.Down;
                    Step();
                }
            
        }
        private void _rotDown(bool no_wall)
        {
          
                if (Maze[X, Y + 1] is Ground && (Maze[_leftwallX, _leftwallY] is Wall || no_wall == false))
                {
                    Y++;
                    _leftwallX = X + 1;
                    _leftwallY = Y;

                }
                else if (no_wall && Maze[X + 1, Y] is Ground)
                {

                    X++;
                    _leftwallX = X;
                    _leftwallY = Y - 1;
                    Rot = Direction.Right;

                }
                else if (no_wall == false && Maze[X - 1, Y] is Ground)
                {

                    X--;
                    _leftwallX = X;
                    _leftwallY = Y + 1;
                    Rot = Direction.Left;

                }
                else
                {
                    Rot = Direction.Right;
                    Step();
                }
            
        }    
        private void _rotRight(bool no_wall)
        {
            
                if (Maze[X + 1, Y] is Ground && (Maze[_leftwallX, _leftwallY] is Wall || no_wall == false))
                {
                    X++;
                    _leftwallX = X;
                    _leftwallY = Y - 1;

                }
                else if (no_wall && Maze[X, Y - 1] is Ground)
                {

                    Y--;
                    _leftwallX = X - 1;
                    _leftwallY = Y;
                    Rot = Direction.Up;

                }
                else if (no_wall == false && Maze[X, Y + 1] is Ground)
                {

                    Y++;
                    _leftwallX = X + 1;
                    _leftwallY = Y;
                    Rot = Direction.Up;

                }
                else
                {
                    Rot = Direction.Up;
                    Step();
                }
            
        }

        public override bool TryToStep()
        {
            throw new NotImplementedException();
        }

    }
}
