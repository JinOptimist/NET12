using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models.GuessTheNumber
{
    public class GuessTheNumberGameAnswerViewModel
    {
        public long Id { get; set; }
        public int IntroducedAnswer { get; set; }
        public long GameId { get; set; }
        public GuessTheNumberGameViewModel Game { get; set; }
    }
}
