using System;
using System.Collections.Generic;
using System.Text;

namespace Net12.Maze
{
    class Coin : BaseCell
    {
        public Coin(int x, int y, int coinCount) : base(x, y)
        {
            CoinCount = coinCount;
        }

        public int CoinCount { get; set; }

        public override bool TryToStep()
        {
            return true;
        }
    }
}
