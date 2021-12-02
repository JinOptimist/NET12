using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;

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
        public DbSet<Movie> Movies { get; set; }

        public DbSet<SuggestedEnemys> SuggestedEnemys { get; set; }
        
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

            modelBuilder.Entity<Movie>()
               .HasOne(m => m.Game)
               .WithMany(g => g.Movies);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
