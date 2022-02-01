using System;

namespace WebMaze.EfStuff.DbModel
{
    [Flags]
    public enum GroupUserLevel
    {
        None = 1,
        Invited = 2,
        Requested = 4,
        Member = 8,
        Admin = 16,

    }
}
