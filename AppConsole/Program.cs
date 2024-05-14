using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Data.Common;

using Configuration;
using Models;
using DbContext;

namespace AppConsole
{
    static class MyLinqExtensions
    {
        public static void Print<T>(this IEnumerable<T> collection)
        {
            collection.ToList().ForEach(item => Console.WriteLine(item));
        }
    }


    class Program
    {
        const int nrItemsSeed = 1000;
        static void Main(string[] args)
        {
            #region run below to test the model only

            Console.WriteLine($"\nSeeding the Model...");
            var _modelList = SeedModel(nrItemsSeed);

            Console.WriteLine($"\nTesting Model...");
            WriteModel(_modelList);
            #endregion


            #region  run below only when Database i created
            Console.WriteLine($"Database type: {csAppConfig.DbSetActive.DbLocation}");
            Console.WriteLine($"Database type: {csAppConfig.DbSetActive.DbServer}");
            Console.WriteLine($"Connection used: {csAppConfig.DbSetActive.DbConnection}");
            Console.WriteLine($"Connection used: {csAppConfig.DbSetActive.DbConnectionString}");
  
            Console.WriteLine($"\nSeeding database...");
            try
            {
                SeedDataBase(_modelList).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: Database could not be seeded. Ensure the database is correctly created");
                Console.WriteLine($"\nError: {ex.Message}");
                Console.WriteLine($"\nError: {ex.InnerException.Message}");
                return;
            }

            Console.WriteLine("\nQuery database...");
            QueryDatabaseAsync().Wait();

            MoreQueryDatabaseAsync().Wait();
            #endregion
       }


        #region Replaced by new model methods
        private static void WriteModel(List<csVehicles> _modelList)
        {
            Console.WriteLine($"NrOfVehicles: {_modelList.Count()}");
        }

        private static List<csVehicles> SeedModel(int nrItems)
        {
            var _seeder = new csSeedGenerator();
            
            //Create a list of friends, adresses and pets
            var _v = _seeder.ItemsToList<csVehicles>(nrItems);
            return _v;
        }
        #endregion

        #region Update to reflect you new Model
        private static async Task SeedDataBase(List<csVehicles> _modelList)
        {
            using (var db = csMainDbContext.DbContext())
            {
                #region move the seeded model into the database using EFC
                foreach (var _v in _modelList)
                {
                    db.Vehicles.Add(_v);
                }
                #endregion

                await db.SaveChangesAsync();
            }
        }

        private static async Task QueryDatabaseAsync()
        {
            Console.WriteLine("--------------");
            using (var db = csMainDbContext.DbContext())
            {
                #region Reading the database using EFC
                var _modelList = await db.Vehicles.ToListAsync();
                #endregion

                WriteModel(_modelList);
            }
        }


        private static async Task MoreQueryDatabaseAsync()
        {
            Console.WriteLine("------More Query --------");
            using (var db = csMainDbContext.DbContext())
            {
                var list = await db.Vehicles.Take(5).ToListAsync();

                Console.WriteLine($"Top 10 vehicles");
                foreach (var _v in list)
                {
                    Console.WriteLine($"{_v}");
                }
            }
        }
        #endregion
    }
}
