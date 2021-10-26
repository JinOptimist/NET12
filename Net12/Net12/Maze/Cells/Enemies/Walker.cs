using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    class Walker : BaseEnemy
    {
        enum Rotation
        {
            Left,
            Right,
            Up,
            Down
        }
        Rotation Rot { get; set; }
        private int _leftwallX;
        private int _leftwallY;
        public Walker(int x, int y, MazeLevel maze) : base(x, y, maze)
        {
            Rot = Rotation.Up;
            _leftwallX = X - 1;
            _leftwallY = Y;

        }

        public override void Step()
        {
            //var poses = Maze.Cells.Where(cell => Math.Abs(this.X - cell.X) == 1 && this.Y == cell.Y || Math.Abs(this.Y - cell.Y) == 1 && this.X == cell.X).OfType<Wall>().ToList();

            if(Rot == Rotation.Up)
            {
                if(Maze[X,Y-1] is Ground && Maze[_leftwallX, _leftwallY] is Wall)
                {
                    Y--;
                    _leftwallX = X-1;
                    _leftwallY = Y;

                }
                else if(Maze[_leftwallX, _leftwallY].Equals(typeof(Wall)) == false && Maze[X-1, Y] is Ground)
                {
                    
                    X--;
                    _leftwallX = X;
                    _leftwallY = Y+1;
                    Rot = Rotation.Left;
                }
                else if (Maze[_leftwallX, _leftwallY].Equals(typeof(Wall)) == false && Maze[X + 1, Y] is Ground)
                {

                    X++;
                    _leftwallX = X;
                    _leftwallY = Y - 1;
                    Rot = Rotation.Right;
                }
                else
                {
                    Rot = Rotation.Left;
                    Step();
                }
            }
            else if (Rot == Rotation.Left)
            {
                if (Maze[X-1, Y] is Ground && Maze[_leftwallX, _leftwallY] is Wall)
                {
                    X--;
                    _leftwallX = X;
                    _leftwallY = Y+1;

                }
                else if (Maze[_leftwallX, _leftwallY].Equals(typeof(Wall)) == false && Maze[X, Y + 1] is Ground)
                {
                    Y++;
                    _leftwallX = X+1;
                    _leftwallY = Y;
                    Rot = Rotation.Down;
                }
                else if (Maze[_leftwallX, _leftwallY].Equals(typeof(Wall)) == false && Maze[X, Y - 1] is Ground)
                {
                    Y--;
                    _leftwallX = X - 1;
                    _leftwallY = Y;
                    Rot = Rotation.Up;
                }
                else
                {
                    Rot = Rotation.Down;
                    Step();
                }
            }
            else if (Rot == Rotation.Down)
            {
                if (Maze[X, Y+1] is Ground && Maze[_leftwallX, _leftwallY] is Wall)
                {
                    Y++;
                    _leftwallX = X+1;
                    _leftwallY = Y;

                }
                else if (Maze[_leftwallX, _leftwallY].Equals(typeof(Wall)) == false && Maze[X+1, Y] is Ground)
                {
                    X++;
                    _leftwallX = X;
                    _leftwallY = Y-1;
                    Rot = Rotation.Right;
                }
                else if (Maze[_leftwallX, _leftwallY].Equals(typeof(Wall)) == false && Maze[X - 1, Y] is Ground)
                {
                    X--;
                    _leftwallX = X;
                    _leftwallY = Y + 1;
                    Rot = Rotation.Left;
                }
                else
                {
                    Rot = Rotation.Right;
                    Step();
                }
            }
            else if (Rot == Rotation.Right)
            {
                if (Maze[X+1, Y] is Ground && Maze[_leftwallX, _leftwallY] is Wall)
                {
                    X++;
                    _leftwallX = X;
                    _leftwallY = Y-1;

                }
                else if (Maze[_leftwallX, _leftwallY].Equals(typeof(Wall)) == false && Maze[X, Y - 1] is Ground)
                {
                    Y--;
                    _leftwallX = X-1;
                    _leftwallY = Y;
                    Rot = Rotation.Up;
                }
                else if (Maze[_leftwallX, _leftwallY].Equals(typeof(Wall)) == false && Maze[X, Y + 1] is Ground)
                {
                    Y++;
                    _leftwallX = X + 1;
                    _leftwallY = Y;
                    Rot = Rotation.Down;
                }
                else
                {
                    Rot = Rotation.Up;
                    Step();
                }
            }


        }

        public override bool TryToStep()
        {
            throw new NotImplementedException();
        }
        
    }
}
