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
        public DbSet<NewCellSuggestion> NewCellSuggestions { get; set; }
        public DbSet<Review> Reviews { get; set; }

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

            modelBuilder.Entity<User>().HasMany(x => x.MyReviews).WithOne(x => x.Creater);
            modelBuilder.Entity<Review>().HasOne(x => x.Creater).WithMany(x => x.MyReviews);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
