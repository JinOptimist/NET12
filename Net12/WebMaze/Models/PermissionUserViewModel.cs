using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.ValidationAttributes;

namespace WebMaze.Models
{
    public class PermissionUserViewModel
    {
        public List<UserViewModel> Users { get; set; }
        public List<PermissionViewModel> Permissions { get; set; }
    }
}