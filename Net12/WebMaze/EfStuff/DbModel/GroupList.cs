using System.Collections.Generic;

namespace WebMaze.EfStuff.DbModel
{
    public class GroupList : BaseModel
    {
        public string Name { get; set; }
        public virtual User Creator { get; set; }
        public virtual List<UserInGroup> Users { get; set; }
    }
}
