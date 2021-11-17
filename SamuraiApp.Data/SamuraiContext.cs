using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Battle> Battles { get; set; }      //Added a Table called Battles so that we can directly interact with it if needed in the Future.
        public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }    //Adds a DBSet that will Map to a View rather than a Table by Specifying below.

        public static readonly ILoggerFactory ConsoleLoggerFactory
            = LoggerFactory.Create(builder =>                                   //Adds a Logger to the Context Class
            {
                builder
                .AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name              //Filter only the Database Commands
                && level == LogLevel.Information)                               //and the Level of Detail needed sp we select only Basic Level of Log Level and nothing more.
                .AddConsole();
            });
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(ConsoleLoggerFactory)
                .EnableSensitiveDataLogging()           //This will display also sensitive data on the Log, which by default is not logged (Incoming Parameter Values)
                .UseSqlServer(
                "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = SamuraiAppData");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId });      //This creates a Composite Key of SamuraiID and BattleID and this helps the EF Core identify the presence of a Many-to-Many Realtionship between Samurai and Battle Tables
            modelBuilder.Entity<Horse>().ToTable("Horses");     //Rather than using DbSet (not used DbSet to indicate the Fact that it is not needed in our Business Logic; so that no one uses it directly)
                                                                //to name the Table we have used the ToTable Function. If we dont use this Function, by default Table name will be Horse (singular as compared to Plural Names we have used so far for all our tables)
            modelBuilder.Entity<SamuraiBattleStat>().HasNoKey().ToView("SamuraiBattleStats");   //Tells the EF Core that this DBSet has no Keys i.e. Keyless Entity and a Table should not be created for it rather a View. Since EF Core cannot create a View, it will leave it to the Mapping of View.
        }
    }
}
