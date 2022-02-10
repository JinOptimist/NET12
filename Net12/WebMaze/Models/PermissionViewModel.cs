using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class PermissionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public virtual List<UserViewModel> UsersWhichHasThePermission { get; set; }

        public int Count { get; set; }
    }
}
