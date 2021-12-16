using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.EfStuff.DbModel
{
    public class User : BaseModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public int Coins { get; set; }
        public int GlobalUserRating { get; set; }

        public virtual List<Perrmission> Perrmissions { get; set; }
        public virtual List<NewCellSuggestion> MyCellSuggestions { get; set; }
        public virtual List<NewCellSuggestion> CellSuggestionsWhichIAprove { get; set; }
        public virtual List<StuffForHero> AddedSStuff { get; set; }
        public virtual List<SuggestedEnemys> MyEnemySuggested { get; set; }
        public virtual List<SuggestedEnemys> EnemySuggestedWhichIAprove { get; set; }
        public virtual List<Review> MyReviews { get; set; }
        public virtual List<Game> MyFavGames { get; set; }
        public virtual List<News> MyNews { get; set; }

        public virtual List<Image> Images { get; set; }
        public virtual List<BugReport> MyBugReports { get; set; }
        public virtual List<GameDevices> MyGameDevices { get; set; }
        public virtual List<MazeDifficultProfile> MazeDifficultProfiles { get; set; }
        public virtual List<MazeLevelWeb> ListMazeLevels { get; set; }
        public virtual List<NewsComment> NewsComments { get; set; }
        public virtual List<MinerField> MinerFields { get; set; }
        public virtual List<ZumaGameField> ZumaGameFields { get; set; }
    }
}
