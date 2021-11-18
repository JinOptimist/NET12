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
        public DbSet<NewCellSuggestion> NewCellSuggestions { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DbSet<News> News { get; set; }
        public WebContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.MyCellSuggestions)
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

            modelBuilder.Entity<User>().HasMany(x => x.MyReviews).WithOne(x => x.Creator);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
