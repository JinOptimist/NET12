using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Coins { get; set; }

        public bool IsActive { get; set; }

        public virtual List<NewCellSuggestion> MyCellSuggestions { get; set; }

        public virtual List<NewCellSuggestion> CellSuggestionsWhichIAprove { get; set; }

        public virtual List<Review> MyReviews { get; set; }
        public virtual List<News> MyNews { get; set; }
    }
}
