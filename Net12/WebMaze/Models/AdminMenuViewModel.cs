using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.CurrencyDto;

namespace WebMaze.Models
{
    public class AdminMenuViewModel
    {
        public List<UserViewModel> Users { get; set; }
        public UserViewModel currUser { get; set; }
        public UserViewModel Coins { get; set; }
        public UserViewModel Id { get; set; }
        public UserViewModel UserName { get; set; }
        public List<PermissionViewModel> permissionViewModels { get; set; }
    }
}