using System;
using System.Collections.Generic;

namespace WebMaze.EfStuff.DbModel
{
    public class Perrmission : BaseModel
    {
        public const string Admin = "Admin";
        public const string NewsCreator = "NewsCreator";
        public const string ForumModerator = "ForumModerator";

        public string Name { get; set; }
        public string Desc { get; set; }

        public virtual List<User> UsersWhichHasThePermission { get; set; }
    }
}