using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.ValidationAttributes;

namespace WebMaze.Models
{
    public class NewsCommentViewModel
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Text { get; set; }
        public string NameOfAuthor { get; set; }
        public long NewsId { get; set; }
    }
}