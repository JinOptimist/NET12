using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class GameDevicesRepository : BaseRepository<GameDevices>
    {
        public GameDevicesRepository(WebContext webContext) : base(webContext)
        {

        }
    }
}
