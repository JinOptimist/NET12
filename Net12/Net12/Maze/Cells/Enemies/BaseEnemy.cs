using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    public abstract class BaseEnemy : Character
    {
        public BaseEnemy(int x, int y, IMazeLevel maze) : base(x, y, maze) 
        {
            X = x;
            Y = y;
            Hp = 30;
        }


        public override bool TryToStep()
        {
            if((Maze.Hero.X == X && Math.Abs(Maze.Hero.Y-Y) == 1) || (Maze.Hero.Y == Y && Math.Abs(Maze.Hero.X - X) == 1))
            {
                Maze.Hero.Hp -= 10;
                Hp -= 10;
                if(Hp <= 0)
                {
                    Maze.Enemies.Remove(this);
                }
            }
            return true;
        }

        public abstract BaseCell BeforeStep();
        public abstract void AfterStep();

        public void GoStep()
        {
            var cell = BeforeStep();
            if(cell is null)
            {
                return;
            }
            var myCell = Maze.GetCellOrUnit(cell.X, cell.Y);
            if(myCell is Character)
            {
                return;
            }
            else
            {
                X = myCell.X;
                Y = myCell.Y;
                AfterStep();
            }

        }

    }
}
