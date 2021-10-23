using System;


namespace Net12.Maze
{
    public class Fountain : BaseCell
    {

        public Fountain(int x, int y, MazeLevel maze) : base(x, y, maze) { }

        public override bool TryToStep()
        {
            if (Maze.Hero.CurrentFatigue > 20)
            {
                Maze.Hero.CurrentFatigue -= 20;
            }
            else if (Maze.Hero.CurrentFatigue <= 20)
            {
                Maze.Hero.CurrentFatigue = 0;
            }


            return true;
        }
    }
}

