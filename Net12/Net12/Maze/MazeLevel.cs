using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    public class MazeLevel
    {
        public List<BaseCell> Cells { get; set; } = new List<BaseCell>();

        public int Width { get; set; }
        public int Height { get; set; }
    }
}
