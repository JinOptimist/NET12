using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    public class Goblin : BaseEnemy
    {
        public Goblin(int x, int y, MazeLevel maze) : base(x, y, maze) { }
        public List<BaseEnemy> AroundGoblin = new List<BaseEnemy>();
        
                

        public override void Step()
        {

        }
        public override bool TryToStep()
        {
            //MazeBuilder.BuildCoin;
            Maze[X, Y] = new Ground(X, Y, Maze);
            return true;
        }
    }
}
