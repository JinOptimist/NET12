using System.Collections.Generic;

namespace WebMaze.Models
{
    public class UsersNotFromGroupViewModel
    {
        public long GroupId { get; set; }
        public List<UserViewModel> NoGroupUsers { get; set; }
    }
}
