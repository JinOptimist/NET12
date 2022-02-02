using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models
{
    public class NewCellSuggestionViewModel
    {
        public long Id { get; set; }
        [MinLength(2, ErrorMessage = "Too short.")]
        public string Title { get; set; }
        [MinLength(2, ErrorMessage = "Too short.")]
        public string Description { get; set; }
        public int MoneyChange { get; set; }
        public int HealtsChange { get; set; }
        public int FatigueChange { get; set; }
        public string UserName { get; set; }
        public int GlobalUserRating { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
