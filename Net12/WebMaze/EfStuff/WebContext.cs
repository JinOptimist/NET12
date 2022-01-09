using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.EfStuff.DbModel.GuessTheNumberDbModel;
using WebMaze.Services.GuessTheNumber;
using WebMaze.Services.GuessTheNumber.Enums;

namespace WebMaze.EfStuff
{
    public class WebContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Gallery { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<NewCellSuggestion> NewCellSuggestions { get; set; }
        public DbSet<StuffForHero> StuffsForHero { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Game> FavGames { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<BugReport> BugReports { get; set; }
        public DbSet<MazeDifficultProfile> MazeDifficultProfiles { get; set; }
        public DbSet<Perrmission> Perrmissions { get; set; }
        public DbSet<SuggestedEnemys> SuggestedEnemys { get; set; }
        public DbSet<MazeLevelWeb> MazeLevelsUser { get; set; }
        public DbSet<MazeCellWeb> CellsModels { get; set; }
        public DbSet<GameDevices> GameDevices { get; set; }
        public DbSet<NewsComment> NewsComments { get; set; }
        public DbSet<GuessTheNumberGame> GuessTheNumberGames { get; set; }
        public DbSet<GuessTheNumberGameAnswer> GuessTheNumberGameAnswers { get; set; }

        public WebContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.MyCellSuggestions)
                .WithOne(x => x.Creater);

            modelBuilder.Entity<User>()
                .HasMany(x => x.MyBugReports)
                .WithOne(x => x.Creater);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Perrmissions)
                .WithMany(x => x.UsersWhichHasThePermission);

            //modelBuilder.Entity<User>()
            //   .HasMany(x => x.CellSuggestionsWhichIAprove)
            //   .WithOne(x => x.Approver);
            modelBuilder.Entity<NewCellSuggestion>()
               .HasOne(x => x.Approver)
               .WithMany(x => x.CellSuggestionsWhichIAprove);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Images)
                .WithOne(x => x.Author);

            modelBuilder.Entity<Image>()
               .HasOne(x => x.Author)
               .WithMany(x => x.Images);

            modelBuilder.Entity<User>()
                .HasMany(x => x.MyEnemySuggested)
                .WithOne(x => x.Creater);

            modelBuilder.Entity<User>()
               .HasMany(x => x.EnemySuggestedWhichIAprove)
               .WithOne(x => x.Approver);

            modelBuilder.Entity<StuffForHero>()
               .HasOne(x => x.Proposer)
               .WithMany(x => x.AddedSStuff);

            modelBuilder.Entity<User>()
                .HasMany(x => x.MyFavGames)
                .WithOne(x => x.Creater);

            modelBuilder.Entity<Game>()
               .HasOne(x => x.Creater)
               .WithMany(x => x.MyFavGames);

            modelBuilder.Entity<User>()
                .HasMany(x => x.MyNews)
                .WithOne(x => x.Author);

            modelBuilder.Entity<User>()
                .HasMany(x => x.MazeDifficultProfiles)
                .WithOne(x => x.Creater);



            modelBuilder.Entity<User>()
               .HasMany(x => x.NewsComments)
               .WithOne(x => x.Author);

            modelBuilder.Entity<News>()
               .HasMany(x => x.NewsComments)
               .WithOne(x => x.News);

            modelBuilder.Entity<User>().HasMany(x => x.ListMazeLevels).WithOne(x => x.Creator);
            modelBuilder.Entity<MazeLevelWeb>().HasMany(x => x.Cells).WithOne(x => x.MazeLevel);
            modelBuilder.Entity<MazeLevelWeb>().HasMany(x => x.Enemies).WithOne(x => x.MazeLevel);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GuessTheNumberGameParameters>()
                .Property(x => x.Difficulty)
                .HasConversion(new EnumToNumberConverter<GuessTheNumberGameDifficulty, int>());

            modelBuilder.Entity<GuessTheNumberGame>()
                .HasOne(x => x.Player)
                .WithMany(x => x.GuessTheNumberGames)
                .HasForeignKey(x => x.PlayerId);

            modelBuilder.Entity<GuessTheNumberGame>()
               .HasOne(x => x.Parameters)
               .WithMany(x => x.Games)
               .HasForeignKey(x => x.ParametersId);

            modelBuilder.Entity<GuessTheNumberGame>()
                .Property(x => x.GameStatus)
                .HasConversion(new EnumToNumberConverter<GuessTheNumberGameStatus, int>());

            modelBuilder.Entity<GuessTheNumberGameAnswer>()
                .HasOne(x => x.Game)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.GameId);

            SeedData(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        protected virtual void SeedData(ModelBuilder modelBuilder)
        {
            SeedGuessTheNumberGameParametersRecords(modelBuilder);
        }

        private static void SeedGuessTheNumberGameParametersRecords(ModelBuilder modelBuilder)
        {
            //static int GetAttemptsCountForRange(int maxRangeNumber, int minRangeNumber)
            //{
            //    var effectiveRangeLength = maxRangeNumber - minRangeNumber + 1;
            //    var attempts = 0;

            //    while (effectiveRangeLength > 0)
            //    {
            //        effectiveRangeLength /= 2;
            //        attempts++;
            //    }

            //    return attempts;
            //}

            //var maxAttemptsForEasy = GetAttemptsCountForRange(
            //            GuessTheNumberGameValues.MinRangeNumberEasy,
            //            GuessTheNumberGameValues.MaxRangeNumberEasy);

            //var maxAttemptsForMedium = GetAttemptsCountForRange(
            //            GuessTheNumberGameValues.MinRangeNumberMedium,
            //            GuessTheNumberGameValues.MaxRangeNumberMedium);

            //var maxAttemptsForHard = GetAttemptsCountForRange(
            //            GuessTheNumberGameValues.MinRangeNumberHard,
            //            GuessTheNumberGameValues.MaxRangeNumberHard);

            var guessTheNumberGameParametersRecords = new[]
            {
                new GuessTheNumberGameParameters
                {
                    Id = 1,
                    Difficulty = GuessTheNumberGameDifficulty.Easy,
                    MinRangeNumber = GuessTheNumberGameValues.MinRangeNumberEasy,
                    MaxRangeNumber = GuessTheNumberGameValues.MaxRangeNumberEasy,
                    GameCost = GuessTheNumberGameValues.GameCostEasy,
                    RewardForWinningTheGame = GuessTheNumberGameValues.RewardForWinningTheGameEasy,
                    MaxAttempts = 4,
                    IsActive = true
                },
                new GuessTheNumberGameParameters
                {
                    Id = 2,
                    Difficulty = GuessTheNumberGameDifficulty.Medium,
                    MinRangeNumber = GuessTheNumberGameValues.MinRangeNumberMedium,
                    MaxRangeNumber = GuessTheNumberGameValues.MaxRangeNumberMedium,
                    GameCost = GuessTheNumberGameValues.GameCostMedium,
                    RewardForWinningTheGame = GuessTheNumberGameValues.RewardForWinningTheGameMedium,
                    MaxAttempts = 7,
                    IsActive = true
                },
                new GuessTheNumberGameParameters
                {
                    Id = 3,
                    Difficulty = GuessTheNumberGameDifficulty.Hard,
                    MinRangeNumber = GuessTheNumberGameValues.MinRangeNumberHard,
                    MaxRangeNumber = GuessTheNumberGameValues.MaxRangeNumberHard,
                    GameCost = GuessTheNumberGameValues.GameCostHard,
                    RewardForWinningTheGame = GuessTheNumberGameValues.RewardForWinningTheGameHard,
                    MaxAttempts = 10,
                    IsActive = true
                }
            };

            modelBuilder.Entity<GuessTheNumberGameParameters>()
                .HasData(guessTheNumberGameParametersRecords);
        }
    }
}
