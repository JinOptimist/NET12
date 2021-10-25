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
        private BaseCell _leftwall;
        public Walker(int x, int y, MazeLevel maze) : base(x, y, maze)
        {
            Rot = Rotation.Up;
            _leftwall = maze[X - 1, Y];

        }

        public override void Step()
        {
            //var poses = Maze.Cells.Where(cell => Math.Abs(this.X - cell.X) == 1 && this.Y == cell.Y || Math.Abs(this.Y - cell.Y) == 1 && this.X == cell.X).OfType<Wall>().ToList();

           if(Rot == Rotation.Up)
            {
                if(Maze[X,Y-1] is Ground && _leftwall is Wall)
                {
                    Y--;
                    _leftwall.X = X-1;
                    _leftwall.Y = Y;

                }
                else if(_leftwall.Equals(typeof(Wall)) == false )
                {
                    X--;
                    _leftwall.X = X;
                    _leftwall.Y = Y+1;
                    Rot = Rotation.Left;
                }
                else
                {
                    Rot = Rotation.Left;
                    Step();
                }
            }
            else if (Rot == Rotation.Left)
            {
                if (Maze[X-1, Y] is Ground && _leftwall is Wall)
                {
                    X--;
                    _leftwall.X = X;
                    _leftwall.Y = Y+1;

                }
                else if (_leftwall.Equals(typeof(Wall)) == false)
                {
                    Y++;
                    _leftwall.X = X+1;
                    _leftwall.Y = Y;
                    Rot = Rotation.Down;
                }
                else
                {
                    Rot = Rotation.Down;
                    Step();
                }
            }
            else if (Rot == Rotation.Down)
            {
                if (Maze[X, Y+1] is Ground && _leftwall is Wall)
                {
                    Y++;
                    _leftwall.X = X+1;
                    _leftwall.Y = Y;

                }
                else if (_leftwall.Equals(typeof(Wall)) == false)
                {
                    X++;
                    _leftwall.X = X;
                    _leftwall.Y = Y-1;
                    Rot = Rotation.Right;
                }
                else
                {
                    Rot = Rotation.Right;
                    Step();
                }
            }
            else if (Rot == Rotation.Right)
            {
                if (Maze[X+1, Y] is Ground && _leftwall is Wall)
                {
                    X++;
                    _leftwall.X = X;
                    _leftwall.Y = Y-1;

                }
                else if (_leftwall.Equals(typeof(Wall)) == false)
                {
                    Y--;
                    _leftwall.X = X-1;
                    _leftwall.Y = Y;
                    Rot = Rotation.Up;
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
