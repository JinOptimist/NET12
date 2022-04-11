using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Models.ValidationAttributes;

namespace WebMaze.Models
{
    public class NewsViewModel
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }

        [StopWord("flat", "bad")]
        [MinLength(5, ErrorMessage = "Too short. SMile")]
        public string Title { get; set; }

        [StopWord("blm")]
        public string Location { get; set; }

        public string Text { get; set; }

        [MaxNewsData]
        [Required(ErrorMessage = "News Date is required")]
        public DateTime EventDate { get; set; }
        public DateTime DateTimeOfPublication { get; set; }
        public bool IsPublished { get; set; }

        public string NameOfAuthor { get; set; }

        public int GlobalUserRating { get; set; }
    }
}
