using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Battle> Battles { get; set; }      //Added a Table called Battles so that we can directly interact with it if needed in the Future.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = SamuraiAppData");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId });      //This creates a Composite Key of SamuraiID and BattleID and this helps the EF Core identify the presence of a Many-to-Many Realtionship between Samurai and Battle Tables
            modelBuilder.Entity<Horse>().ToTable("Horses");     //Rather than using DbSet (not used DbSet to indicate the Fact that it is not needed in our Business Logic; so that no one uses it directly)
                                                                //to name the Table we have used the ToTable Function. If we dont use this Function, by default Table name will be Horse (singular as compared to Plural Names we have used so far for all our tables)
        }
    }
}
