using System;
using System.Threading;

namespace WebMaze.Models
{
    public class UserMazeActivity
    {
        public long MazeId { get; set; }
        public DateTime LastActivity { get; set; }
        public bool IsActive { get; set; }
    }
}
