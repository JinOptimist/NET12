using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    public class BullEnemy : BaseEnemy
    {
        private Random random = new Random();
        public BullEnemy(int x, int y, MazeLevel maze) : base(x, y, maze) { }

        public override void Step()
        {
            
        }

        public override bool TryToStep()
        {
            Maze.Hero.Healt = Maze.Hero.MaxHealt;
            return false;
        }

        //private BaseCell GetRandom(List<BaseCell> cells)
        //{
        //    var index = random.Next(cells.Count);
        //    return cells[index];
        //}
    }
}
