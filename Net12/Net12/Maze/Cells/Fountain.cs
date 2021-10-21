using System;


namespace Net12.Maze
{
    public class Fountain : BaseCell
    {

        public Fountain(int x, int y, MazeLevel maze) : base(x, y, maze) { }

        public override bool TryToStep()
        {
            Maze.Hero.CurrentFatigue -= 5;


            return true;
        }
    }
}

