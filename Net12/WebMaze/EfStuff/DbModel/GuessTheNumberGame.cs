using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Services.GuessTheNumber;
using WebMaze.Services.GuessTheNumber.Enums;

namespace WebMaze.EfStuff.DbModel.GuessTheNumberDbModel
{
    public class GuessTheNumberGame : BaseModel
    {
        public int GuessedNumber { get; set; }
        public DateTime GameDate { get; set; }
        public int AttemptNumber { get; set; }
        public GuessTheNumberGameStatus GameStatus { get; set; }        
        public long PlayerId { get; set; }
        public virtual User Player { get; set; }
        public virtual List<GuessTheNumberGameAnswer> Answers { get; set; }
        public long ParametersId { get; set; }
        public virtual GuessTheNumberGameParameters Parameters { get; set; }
    }
}
