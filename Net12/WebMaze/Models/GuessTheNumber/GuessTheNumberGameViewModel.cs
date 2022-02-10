using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Services.GuessTheNumber;

namespace WebMaze.Models.GuessTheNumber
{
    public class GuessTheNumberGameViewModel
    {
        public DateTime StartDateGame { get; set; }
        public GuessTheNumberGameStatus GameStatus { get; set; }
        public int AttemptNumber { get; set; }
        public int GuessedNumber { get; set; }
        public string PlayerName { get; set; }
        public List<GuessTheNumberGameAnswerViewModel> Answers { get; set; }
        public GuessTheNumberGameParametersViewModel Parameters { get; set; }
    }
}
