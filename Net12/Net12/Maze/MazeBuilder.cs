using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    public class MazeBuilder
    {
        public MazeLevel Build(int width, int height)
        {
            var maze = new MazeLevel();

            maze.Width = width;
            maze.Height = height;

            for (int y = 0; y < maze.Height; y++)
            {
                for (int x = 0; x < maze.Width; x++)
                {
                    var wall = new Wall(x, y);
                    maze.Cells.Add(wall);
                }
            }

            return maze;
        }
    }
}
