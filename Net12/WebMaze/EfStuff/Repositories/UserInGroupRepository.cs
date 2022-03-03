using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class UserInGroupRepository : BaseRepository<UserInGroup>
    {
        public UserInGroupRepository(WebContext webContext) : base(webContext)
        {
        }
    }
}
