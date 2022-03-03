using WebMaze.EfStuff.DbModel;

namespace WebMaze.EfStuff.Repositories
{
    public class GroupListRepository : BaseRepository<GroupList>
    {
        public GroupListRepository(WebContext webContext) : base(webContext)
        {
        }
    }
}
