using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class UserViewModel
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public int Coins { get; set; }
    }
}
