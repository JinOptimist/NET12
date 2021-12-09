using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells
{
    public class WeakWall : Wall

    {
        public int _vitalityOfWeakWall = 3;

        public WeakWall(int x, int y, IMazeLevel maze) : base(x, y, maze) { }

        public override bool TryToStep()
        {
            _vitalityOfWeakWall--;

            if (_vitalityOfWeakWall <= 0)
            {
                Maze[X, Y] = new Ground(X, Y, Maze);
            }

            return false;

        }
    }
}
