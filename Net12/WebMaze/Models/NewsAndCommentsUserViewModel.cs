using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.ValidationAttributes;

namespace WebMaze.Models
{
    public class NewsAndCommentsUserViewModel
    {
        public List< NewsViewModel> News { get; set; }
        public List<NewsCommentViewModel> NewsComments { get; set; }
    }
}