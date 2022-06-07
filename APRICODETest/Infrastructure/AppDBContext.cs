using APRICODETest.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APRICODETest.Infrastructure
{
    public class AppDBContext : DbContext
    {
        public AppDBContext (DbContextOptions<AppDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Genre> Genres { get; set; }
        //public DbSet<GameGenre> GameGenres { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<GameGenre>().HasKey(k => new { k.GameId, k.GenreId });
            modelBuilder.Entity<Game>().HasMany(x => x.Genres).WithMany(x => x.Games).UsingEntity<GameGenre>(x => x.HasOne(x => x.Genre).WithMany().HasForeignKey(x => x.GenreId), x => x.HasOne(x => x.Game).WithMany().HasForeignKey(x => x.GameId));
        }
    }
}
