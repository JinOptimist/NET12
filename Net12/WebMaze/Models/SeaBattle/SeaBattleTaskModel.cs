using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebMaze.Services;

namespace WebMaze.Models
{
    public class SeaBattleTaskModel
    {
        public long Id { get; set; }
        public bool IsActiveUser { get; set; }
        public int SecondsToEnemyTurn { get; set; } = SeaBattleService.SECONDS_TO_ENEMY_TURN;
        public DateTime LastActiveUserDateTime { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}
