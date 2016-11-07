using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LinqInEntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CarDB>());
            InsertData();
            QueryData();


            //var cars = ConvertCSV("fuel.csv");
            //foreach (var i in cars)
            //{
            //    Console.WriteLine($"{i.Name} : {i.Manufacturer}");
            //}
        }

        private static void QueryData()
        {
            var db = new CarDB(); // create a instance of car db



            db.Database.Log = Console.WriteLine;

            var query = db.Cars.OrderByDescending(c => c.Combined)
                .ThenBy(c => c.Name)
                .Select(c => new { c.Name, c.Manufacturer, c.Id })

                .Take(10)
                ;

            Console.WriteLine(query.Count());

            foreach (var c in query)
            {
                Console.WriteLine($"{c.Id,-3} : {c.Name,20} : {c.Manufacturer,30} : ");
            }


            var q = db.Cars.GroupBy(C => C.Manufacturer).Select(g => new
            {
                ManuName = g.Key, // intellisence not work
                Cars = g.OrderByDescending(c => c.Combined).ThenBy(c=>c.Name).Take(2)
            });


            foreach (var grp in q)
            {
                Console.WriteLine(grp.ManuName);
                foreach (var c in grp.Cars)
                {
                    Console.WriteLine($"\t{c.Name} : { c.Combined}");
                }
            }

        }

        private static void InsertData()
        {
            var cars = ConvertCSV("fuel.csv"); // call to read data in memory
            var db = new CarDB();

            //db.Database.Log = Console.WriteLine;

            if (!db.Cars.Any())
            {
                foreach (var car in cars)
                {
                    db.Cars.Add(car);
                }
                db.SaveChanges();
            }


        }

        private static List<Car> ConvertCSV(string path)
        {

            return
                File.ReadAllLines(path)
                .Skip(1)
                .Where(c => c.Length > 1)
                .Select(c =>
                {
                    var columns = c.Split(',');
                    return new Car
                    {
                        Year = int.Parse(columns[0]),
                        Manufacturer = columns[1],
                        Name = columns[2],
                        Displacement = Double.Parse(columns[3]),
                        Cylinders = int.Parse(columns[4]),
                        City = int.Parse(columns[5]),
                        Highway = int.Parse(columns[6]),
                        Combined = int.Parse(columns[7])
                    };
                })
                .ToList();


        }
    }
}
