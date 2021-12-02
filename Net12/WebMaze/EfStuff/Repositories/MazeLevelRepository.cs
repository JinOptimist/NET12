using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class MazeLevelRepository : BaseRepository<MazeLevelModel>
    {
        public MazeLevelRepository(WebContext webContext) : base(webContext)
        {
        }
    }
}
