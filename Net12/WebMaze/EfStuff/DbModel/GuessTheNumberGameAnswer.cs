using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel.GuessTheNumberDbModel
{
    public class GuessTheNumberGameAnswer : BaseModel
    {        
        public int IntroducedAnswer { get; set; }       
       
        public long GameId { get; set; }
        public virtual GuessTheNumberGame Game { get; set; }
    }
}
