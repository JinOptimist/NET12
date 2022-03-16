using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class SeaBattleTaskModel
    {
        public long Id { get; set; }
        public bool UserIsActive { get; set; }
        public int SecondsToEnemyTurn { get; set; } = 10;
        public int TimerIsActiveUser { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}
