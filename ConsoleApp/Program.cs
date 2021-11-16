using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace ConsoleApp
{
    internal class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        static void Main(string[] args)
        {
            //_context.Database.EnsureCreated();           //Ensures that a Database Exist. If it does not then it Creates the corresponding Database.
            //GetSamurais("Before Add:");
            //AddSamurai();
            //GetSamurais("After Add:");
            //InsertMultipleSamurais();
            //InsertVariousTypes();
            //GetSamuraisSimpler();
            //QueryFilters();
            //GetSamurais("After Add:");
            //RetrieveAndUpdateSamurai();
            //RetrieveAndUpdateMultipleSamurais();
            //MultipleDatabaseOperations();
            //RetrieveAndDeleteASamurai();
            //InsertBattle();
            //QueryAndUpdateBattle_Disconnected();
            Console.Write("Press any Key...");
            Console.ReadKey();
        }

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
    }
}
