using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    internal class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        static void Main(string[] args)
        {
            #region Update/Delete/Retrieve Operations; Filtering/Aggregating Queries
            /*
            _context.Database.EnsureCreated();           //Ensures that a Database Exist. If it does not then it Creates the corresponding Database.
            GetSamurais("Before Add:");
            AddSamurai();
            GetSamurais("After Add:");
            InsertMultipleSamurais();
            InsertVariousTypes();
            GetSamuraisSimpler();
            QueryFilters();
            GetSamurais("After Add:");
            RetrieveAndUpdateSamurai();
            RetrieveAndUpdateMultipleSamurais();
            MultipleDatabaseOperations();
            RetrieveAndDeleteASamurai();
            InsertBattle();
            QueryAndUpdateBattle_Disconnected(); */
            #endregion

            #region Eager Loading (Include) / Projecting (Select) / Explicit Loading (Load) of Related Data / Filtering and Sorting Related Data
            /*
            InsertNewSamuraiWithAQuote();
            InsertNewSamuraiWithManyQuotes();
            AddQuoteToExistingSamuraiWhileTracked();
            AddQuoteToExistingSamuraiNotTracked(2);
            AddQuoteToExistingSamuraiNotTracked_Easy(2);
            EagerLoadSamuraiWithQuotes();
            ProjectSomeProperties();
            ProjectSamuraiWithQuotes();
            ExplicitLoadQuotes();
            FilteringWithRelatedData();
            ModifyingRelatedDataWhenTracked();
            ModifyingRealtedDataWhenNotTracked();
            JoinBattleAndSamurai();
            EnlistSamuraiIntoABattle();
            GetSamuraiWithBattles();
            RemoveJoinBetweenSamuraiAndBattleSimple();
            AddNewSamuraiWithHorse();
            AddNewHorseToSamuraiUsingId();
            AddNewHorseToSamuraiObject();
            AddNewHorseToDisconnectedSamuraiObject();
            ReplaceHorse();
            GetSamuraisWithHorse();
            GetHorsewithSamurai();
            GetSamuraiWithClan();
            GetClanWithSamurais(); */
            #endregion

            #region Query using View / Stored Procedure / Keyless Entities / Raw SQL (Interpolated) / Database Execute Raw SQL
            /*
            QuerySamuraiBattleStats();
            QueryUsingRawSql();
            QueryUsingRawlSqlWithInterpolation();
            DangerQueryUsingRawlSqlWithInterpolation();
            QueryUsingRawSqlStoredProcParameters();
            InterpolatedQueryUsingRawSqlStoredProcParameters();
            ExecuteSomeRawSql();
            */
            #endregion
            
            Console.Write("Press any Key...");
            Console.ReadKey();
        }

        #region Insert, Update, Delete Operations; Filtering/Aggregating Queries
        private static void InsertMultipleSamurais()
        {
            var samurai = new Samurai { Name = "Sampson" };
            var samurai2 = new Samurai { Name = "Tasha" };
            var samurai3 = new Samurai { Name = "Number3" };
            var samurai4 = new Samurai { Name = "Number4" };
            _context.Samurais.AddRange(samurai, samurai2, samurai3, samurai4);
            _context.SaveChanges();
        }

        private static void AddSamurai()
        {
            var samurai = new Samurai { Name = "Julie" }; //var samurai = new Samurai { Name = "Sampson" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertVariousTypes()
        {
            var samurai = new Samurai { Name = "Kikuchio" };
            var clan = new Clan { ClanName = "Imperial Clan" };
            _context.AddRange(samurai, clan);       //Different DBSet Types can be added directly without the need of specifying the DBSet. This feAture has beenadded in EF Core.
            _context.SaveChanges();                 //Instead of _context.Samurai.Add() & _context.Clan.Add()
        }

        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais.ToList();
            Console.WriteLine($"{text} : Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void GetSamuraisSimpler()
        {
            //var samurais = _context.Samurais.ToList();      //Gets all the List of Samurais from the Database
            var query = _context.Samurais;              //Separating out the Query part from the LINQ Method ToList()
            var samurais = query.ToList();

            foreach (var samurai in query)          //Such type of Operation should be avaoided coz for each Print Statement it is retrieving from the DB.
            {                                       //The DB Connection is Open during the Entire Loop. For Many Operations, this runs into performance Issues.
                Console.WriteLine(samurai.Name);
            }
        }

        private static void QueryFilters()
        {
            var name = "Sampson";
            var samurais = _context.Samurais.Where(s => s.Name == name).ToList();
            var samurai = _context.Samurais.Find(2);        //Not a LINQ Method but a DBSet Method. Executes Immediately
            var filter = "J%";
            var samurais2 = _context.Samurais.Where(s => EF.Functions.Like(s.Name, filter)).ToList();
            var samurais3 = _context.Samurais.Where(s => s.Name == name).SingleOrDefault();     //Gets the Top 1 (Translates to SQL as this) Result if present or Null
            var samurais4 = _context.Samurais.FirstOrDefault(s => s.Name == name);              // Above and Below Query Results are same.

            var last = _context.Samurais.OrderBy(s => s.Id).LastOrDefault(s => s.Name == name);     //Note that while using LastorDefault, OrderBy has to be used becoz it needs to get Last by some Criteria as in Data needs to be in some order.
            //The following will throw an Exception
            //var lastNoOrder = _context.Samurais.LastOrDefault(s => s.Name == name);
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.SaveChanges();
        }

        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurais = _context.Samurais.Skip(1).Take(4).ToList();
            samurais.ForEach(s => s.Name += "San");
            _context.SaveChanges();
        }

        private static void MultipleDatabaseOperations()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.Add(new Samurai { Name = "Koklein" });
            _context.SaveChanges();             
        }

        private static void RetrieveAndDeleteASamurai()
        {
            var samurai = _context.Samurais.Find(7);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void InsertBattle()
        {
            _context.Battles.Add(new Battle
            {
                Name = "Battle of Okehazama",
                StartDate = new DateTime(1560, 05, 01),
                EndDate = new DateTime(1560, 06, 15)
            });
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattle_Disconnected()
        {
            var battle = _context.Battles.AsNoTracking().FirstOrDefault();      //Disable Trcaking so that Conetxt does not keep track of any changes done to the Object. Should normally be used for Read-Only Operations not involving any Data Manipulation.
            battle.EndDate = new DateTime(1560, 06, 30);
            using (var newContextInstance = new SamuraiContext())       //Creates a New Instance to mimic the scenario when there is no tracking and the context instance has no clue as to what has been udpated, added or deleted
            {
                newContextInstance.Battles.Update(battle);          //All Properties are sent for Update in a Disconnected Update Operation inspite of only one property being changed, as it has no tracking to see what has changed. It only knows that something has changed.
                newContextInstance.SaveChanges();
            }
        }

        #endregion

        #region Eager Loading (Include) / Projecting (Select) / Explicit Loading (Load) of Related Data / Filtering and Sorting Related Data
        private static void InsertNewSamuraiWithAQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "I've come to save you" }
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewSamuraiWithManyQuotes()
        {
            var samurai = new Samurai
            {
                Name = "Kyuzo",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "Watch out for my Sharp Sword!" },
                    new Quote { Text = "I told you to watch out for the sharp sword! Oh Well!" }
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you're happy that I've Saved you!"
            });
            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "Now that I saved you, will you feed me dinner?"
            });
            using(var newContext = new SamuraiContext())
            {
                //newContext.Samurais.Update(samurai);             //Note that even though its a new context, it will check and see that the Child(Quote) has no Key, so it will add it a new Record with the Foreign Key as the Parent Key . Since we are calling the Update it will update the Samurai also.     
                newContext.Samurais.Attach(samurai);            //Rather than updating the Samurai we just attach the Samurai, so it will only update the Quote Object and not the Samurai Again.
                newContext.SaveChanges();               
            }
        }

        private static void AddQuoteToExistingSamuraiNotTracked_Easy(int samuraiId)
        {
            var quote = new Quote
            {
                Text = "Now that I saved you, will you feed me dinner again?",
                SamuraiId = samuraiId
            };
            using(var newContext = new SamuraiContext())
            {
                newContext.Quotes.Add(quote);
                newContext.SaveChanges();
            }
        }

        private static void EagerLoadSamuraiWithQuotes()
        {
            var samuraiWithQuotes = _context.Samurais.Where(s => s.Name.Contains("Julie"))
                                                     .Include(s => s.Quotes)
                                                     .Include(s => s.Clan)
                                                     .FirstOrDefault();
        }

        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
            var idsAndNames = _context.Samurais.Select(s => new IdAndName { Id = s.Id, Name = s.Name }).ToList();
        }

        //Creating a Data Type of Ids and Names for depicting anonymous vs Data Type in Select/Projection.
        public struct IdAndName
        {
            public IdAndName(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id;
            public string Name;
        }

        private static void ProjectSamuraiWithQuotes()
        {
            var somePropertiesWithQuotes = _context.Samurais
                .Select(s => new { s.Id, s.Name, s.Quotes })
                .ToList();
            var somePropertiesWithQuotes2 = _context.Samurais
                .Select(s => new { s.Id, s.Name, s.Quotes.Count })
                .ToList();
            var somePropertiesWithQuotes3 = _context.Samurais
                .Select(s => new {s.Id, s.Name,
                HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))})
                .ToList();
            var samuraisWithHappyQuotes = _context.Samurais
                .Select(s => new
                {
                    Samurai = s,
                    HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))         //An Example of how with Select you can filter out certain part of the other Attribute (i.e. Filtering the Related Data) as compared to the Includes, which gets all the Data
                }).ToList();

            //var firstSamurai = samuraisWithHappyQuotes[0].Samurai.Name += " The Happiest";      //Just Modifying Locally and not saving changes to show how even though anonymous types are not tracked during projection, if Entities are a part of the anonymous type they are tracked
                                                                                                //Kept a Watch on _context.ChangeTracker.Entries -> Results View
        }

        private static void ExplicitLoadQuotes()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name.Contains("Julie"));
            _context.Entry(samurai).Collection(s => s.Quotes).Load();       //Collection becoz there exists a one-to-many realtionship between Samurai and Quotes
            _context.Entry(samurai).Reference(s => s.Horse).Load();         //Reference becoz there exist a one-to-one relationship between Samurai and Horse
        }

        private static void FilteringWithRelatedData()
        {
            var samurais = _context.Samurais
                                   .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))
                                   .ToList();
        }

        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault();
            samurai.Quotes[0].Text = " Did you hear that?";
            _context.Quotes.Remove(samurai.Quotes[2]);
            _context.SaveChanges();
        }

        private static void ModifyingRealtedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault();
            var quote = samurai.Quotes[0];
            quote.Text = "Did you hear that again?";
            using (var newContext = new SamuraiContext())
            {
                //newContext.Quotes.Update(quote);        //This Updates all the Quotes of the Samurai as well as the Update of Samurai Details, since each quote is Linked to a Samurai (and this Samurai internally has Quotes attached to it)
                newContext.Entry(quote).State = EntityState.Modified;       //Here we are explicitly setting the state of that Paritcular QUote Object to Modify. This is done without affecting the states of other Linked Attributes like Samurai etc. Hence during Update now, only the Quote is Updated as expected.
                newContext.SaveChanges();
            }
        }

        private static void JoinBattleAndSamurai()
        {
            //Samurai and Battle already exist and we have their Ids
            var sbJoin = new SamuraiBattle { SamuraiId = 2, BattleId = 1 };
            _context.Add(sbJoin);       //Note that since we dont have a DBContext of SamuraiBattle we cannot add it against that through DBContext i.e. _context.SamuraiBattle.Add(). This _context.Add() method allows us to directly add to the DB without DBSet
            _context.SaveChanges();
        }
        
        private static void EnlistSamuraiIntoABattle()
        {
            var battle = _context.Battles.Find(1);
            battle.SamuraiBattles
                .Add(new SamuraiBattle { SamuraiId = 3 });      //The Context will automatically pick the Battle Id from the current active one and add it into the Samurai Battle
            _context.SaveChanges();
        }

        private static void GetSamuraiWithBattles()
        {
            var samuraiWithBattle = _context.Samurais
                .Include(s => s.SamuraiBattles)
                .ThenInclude(sb => sb.Battle)
                .FirstOrDefault(samurai => samurai.Id == 2);

            var samuraiWithBattlesCleaner = _context.Samurais.Where(s => s.Id == 2)
                .Select(s => new
                {
                    Samurai = s,
                    Battle = s.SamuraiBattles.Select(sb => sb.Battle)
                }).FirstOrDefault();
                                             
        }

        private static void RemoveJoinBetweenSamuraiAndBattleSimple()
        {
            var join = new SamuraiBattle { BattleId = 1, SamuraiId = 2 };
            _context.Remove(join);
            _context.SaveChanges();
        }

        private static void AddNewSamuraiWithHorse()
        {
            var samurai = new Samurai { Name = "Jina Ujichika" };
            samurai.Horse = new Horse { Name = "Silver" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddNewHorseToSamuraiUsingId()
        {
            var horse = new Horse { Name = "Scout", SamuraiId = 2 };
            _context.Add(horse);
            _context.SaveChanges();
        }

        private static void AddNewHorseToSamuraiObject()
        {
            var samurai = _context.Samurais.Find(1);
            samurai.Horse = new Horse { Name = "Black Beauty" };
            _context.SaveChanges();
        }

        private static void AddNewHorseToDisconnectedSamuraiObject()
        {
            var samurai = _context.Samurais.AsNoTracking().FirstOrDefault(s => s.Id == 4);
            samurai.Horse = new Horse { Name = "Mr. Ed" };
            using (var newContext = new SamuraiContext())
            {
                newContext.Attach(samurai);         //Becoz the Horse does not have an ID, EF Core understands that it is the one to be Inserted and not the Samurai who already has an Id (This is done throught the Attach Function)
                newContext.SaveChanges();
            }
        }

        private static void ReplaceHorse()
        {
            var samurai = _context.Samurais.Include(s => s.Horse).FirstOrDefault(s => s.Id == 4);
            //var samurai = _context.Samurais.Find(4);            //Has a Horse, but since the Horse is not loaded in memory, it cannot delete the Old Horse and when i tries to insert the New Horse it gets an Exception of Duplicate Key
                                                                  //Above Statement will give an Error.  
            samurai.Horse = new Horse { Name = "Trigger" };     //Since our DB is defined in such a way that a Horse cannot exist without a Samurai due to Foreign Key Relationship, therefore it first deletes the old Horse and then Inserts the New Horse.
            _context.SaveChanges();
        }

        private static void GetSamuraisWithHorse()
        {
            var samurai = _context.Samurais.Include(s => s.Horse).ToList();
        }

        private static void GetHorsewithSamurai()       //This part is a liitle tricky becoz the Horse does not have a Navigational Property back to Samurais i.e. it does not have public Samurai samurai; it only has the Samurai Id
        {
            var horseWithoutSamurai = _context.Set<Horse>().Find(3);    //Set Property helps to create a Temp Set in this case as we do not have a DBSet of Horse. This will just get the Horse and not associated Samurais

            var horseWithSamurai = _context.Samurais.Include(s => s.Horse)      //Start with the Samurais and Include the Horse and then Filter the one whose Id is 3. Gets a Single Horse with its corresponding Samurai only.
                .FirstOrDefault(s => s.Horse.Id == 3);

            var horseWithSamurais = _context.Samurais           //Gets all the Samurais who have Horses.
                .Where(s => s.Horse != null)
                .Select(s => new { Horse = s.Horse, Samurai = s })
                .ToList();
        }

        private static void GetSamuraiWithClan()
        {
            var samurai = _context.Samurais.Include(s => s.Clan).FirstOrDefault();
        }

        private static void GetClanWithSamurais()
        {
            //var clan = _context.Clans.Include(c => c.??)  //This is not Possible as we dont have a Navigational Property to Samurais
            var clan = _context.Clans.Find(1);
            var samuraisForClan = _context.Samurais.Where(s => s.Clan.Id == 1).ToList();
        }

        #endregion

        #region Query using View / Stored Procedure / Keyless Entities / Raw SQL (Interpolated) / Database Execute Raw SQL
        private static void QuerySamuraiBattleStats()
        {
            //var stats = _context.SamuraiBattleStats.ToList();            
            var firstStat = _context.SamuraiBattleStats.FirstOrDefault();
            var sampsonStat = _context.SamuraiBattleStats
                              .Where(s => s.Name == "SampsonSan").FirstOrDefault();

            //var findone = _context.SamuraiBattleStats.Find(2);   //Makes no Sense, Since this is a Keyless Entity and Find, whick works based on a Key, wont work here
                                                        //No Compile Time Error, Since its a Valid Method of DbSet; Run Time Error
        }

        private static void QueryUsingRawSql()
        {
            var samurais = _context.Samurais.FromSqlRaw("Select * From Samurais").ToList();

            var samurais2 = _context.Samurais.FromSqlRaw("Select Id, Name, ClanId From Samurais")
                                    .Include(s => s.Quotes).ToList();
            
            //var samurais = _context.Samurais.FromSqlRaw("Select Name From Samurais").ToList();                                //Not Allowed as you have to specify all the Properties of the Entity
            //var samurais = _context.Samurais.FromSqlRaw("Select Id, Name, Quotes, Clan, SamuraiBattles, Horse From Samurais").ToList();      //Cannot Select Navigation Properties in Raw SQL
        }

        private static void QueryUsingRawlSqlWithInterpolation()
        {
            string name = "Kikuchio";
            var samurais = _context.Samurais
                .FromSqlInterpolated($"Select * From Samurais Where Name = {name}")
                .ToList();
        }

        private static void DangerQueryUsingRawlSqlWithInterpolation()      //In this Method we try to send an Interpolated String through the Raw SQL Method
        {                                           //Shoudl not be used as it is susceptible to SQL Injection Attacks.
            string name = "Kikuchio";
            var samurais = _context.Samurais
                .FromSqlRaw($"Select * From Samurais Where Name = '{name}'")
                .ToList();
        }

        private static void QueryUsingRawSqlStoredProcParameters()
        {
            var text = "Happy";
            var samurais = _context.Samurais.FromSqlRaw(
                "EXEC dbo.SamuraisWhoSaidAWord {0}", text).ToList();
        }

        private static void InterpolatedQueryUsingRawSqlStoredProcParameters()
        {
            var text = "Happy";
            var samurais = _context.Samurais.FromSqlInterpolated(
                $"EXEC dbo.SamuraisWhoSaidAWord {text}").ToList();
        }

        private static void ExecuteSomeRawSql()
        {
            var samuraiId = 22;
            var x = _context.Database.ExecuteSqlRaw("EXEC DeleteQuotesForSamurai {0}", samuraiId);

            samuraiId = 31;
            var y = _context.Database.ExecuteSqlRaw($"EXEC DeleteQuotesForSamurai {samuraiId}");
        }
        #endregion
    }
}
