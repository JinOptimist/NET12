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
            left,
            right,
            up,
            down
        }
        Rotation Rot { get; set; }
        private BaseCell _poswall;
        public Walker(int x, int y, MazeLevel maze) : base(x, y, maze)
        {
            Rot = Rotation.left;
            _poswall = maze[X - 1, Y];

        }

        public override void Step()
        {
            //var poses = Maze.Cells.Where(cell => Math.Abs(this.X - cell.X) == 1 && this.Y == cell.Y || Math.Abs(this.Y - cell.Y) == 1 && this.X == cell.X).OfType<Wall>().ToList();

            if (Rot == Rotation.left)
            {
                if (Maze[this.X, this.Y - 1] is Ground && _poswall is Wall)
                {
                    Y--;
                    _poswall = Maze[X - 1, Y];
                }
                else if (Maze[this.X - 1, this.Y] is Ground)
                {
                    X--;
                    Rot = Rotation.down;
                    _poswall = Maze[X, Y + 1];
                }
                else
                {
                    Rot = Rotation.down;
                }

            }
            if (Rot == Rotation.down)
            {
                if (Maze[this.X - 1, this.Y] is Ground && _poswall is Wall)
                {
                    X--;
                    _poswall = Maze[X, Y + 1];
                }
                else if (Maze[this.X, this.Y + 1] is Ground)
                {
                    Y--;
                    Rot = Rotation.right;
                    _poswall = Maze[X + 1, Y];
                }
                else
                {
                    Rot = Rotation.right;
                }

            }
            if (Rot == Rotation.right)
            {
                if (Maze[this.X, this.Y + 1] is Ground && _poswall is Wall)
                {
                    Y--;
                    _poswall = Maze[X, Y + 1];
                }
                else if (Maze[this.X + 1, this.Y] is Ground)
                {
                    X++;
                    Rot = Rotation.up;
                    _poswall = Maze[X, Y + 1];
                }
                else
                {
                    Rot = Rotation.up;
                }

            }
            if (Rot == Rotation.up)
            {
                if (Maze[this.X + 1, this.Y] is Ground && _poswall is Wall)
                {
                    Y--;
                    _poswall = Maze[X + 1, Y];
                }
                else if (Maze[this.X, this.Y+1] is Ground)
                {
                    X++;
                    Rot = Rotation.left;
                    _poswall = Maze[X-1, Y];
                }
                else
                {
                    Rot = Rotation.left;
                }

            }


        }

        public override bool TryToStep()
        {
            throw new NotImplementedException();
        }
    }
}
