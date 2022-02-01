using System.Collections.Generic;

namespace WebMaze.EfStuff.DbModel
{
    public class UserInGroup : BaseModel
    {
        public virtual User User { get; set; }
        public virtual GroupList Group { get; set; }
        public GroupUserLevel UserLevel { get; set; }
    }
}
