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

        }

        private static void InsertData()
        {
            var cars = ConvertCSV("fuel.csv"); // call to read data in memory
            var db = new CarDB();

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
