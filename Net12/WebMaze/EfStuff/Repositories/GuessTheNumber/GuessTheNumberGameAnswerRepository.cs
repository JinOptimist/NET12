using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel.GuessTheNumberDbModel;

namespace WebMaze.EfStuff.Repositories.GuessTheNumber
{
    public class GuessTheNumberGameAnswerRepository : BaseRepository<GuessTheNumberGameAnswer>
    {
        public GuessTheNumberGameAnswerRepository(WebContext webContext) : base(webContext)
        {

        }
    }
}
