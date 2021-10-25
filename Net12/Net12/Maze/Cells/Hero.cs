using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    public class Hero : BaseCell
    {
        public int Money { get; set; }

        public int Hp { get; set; }

        public int Max_hp { get; set; }

        public Hero(int x, int y, MazeLevel maze, int hp, int max_hp) : base(x, y, maze) {
            Hp = hp;

            Max_hp = max_hp;
        }

        public override bool TryToStep()
        {
            throw new NotImplementedException();
        }
    }
}
