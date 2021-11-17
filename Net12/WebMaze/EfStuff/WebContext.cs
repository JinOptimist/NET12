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

        public WebContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.Images)
                .WithOne(x => x.Author);

            modelBuilder.Entity<Image>()
               .HasOne(x => x.Author)
               .WithMany(x => x.Images);

            base.OnModelCreating(modelBuilder);
        }
    }
}
