namespace WebMaze.Models
{
    public class UserInGroupViewModel
    {
        public long Id { get; set; }
        public UserViewModel User { get; set; }
        public GroupListViewModel Group { get; set; }
    }
}
