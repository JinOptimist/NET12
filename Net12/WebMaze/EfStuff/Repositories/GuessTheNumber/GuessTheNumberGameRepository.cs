using WebMaze.EfStuff.DbModel.GuessTheNumber;
using System.Linq;
using WebMaze.Services.GuessTheNumber;

namespace WebMaze.EfStuff.Repositories.GuessTheNumber
{
    public class GuessTheNumberGameRepository : BaseRepository<GuessTheNumberGame>
    {
        public GuessTheNumberGameRepository(WebContext webContext) : base(webContext)
        {

        }
        public virtual GuessTheNumberGame GetCurrentGame(long userId)
        {
            return _dbSet.SingleOrDefault(g =>
                    g.GameStatus == GuessTheNumberGameStatus.NotFinished
            &&
            g.Player.Id == userId);
        }





    }
}
