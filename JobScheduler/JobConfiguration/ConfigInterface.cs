using System;
using System.Linq;
using JobLibrary;
using FactoryLibrary;

namespace JobConfiguration
{
    public class ConfigInterface
    {
        static void Main(string[] args)
        {
            var ConfigTools = new ConfigInterface();
            ConfigTools.ConfigJobs();
        }

        private void ConfigJobs()
        {
            var dbFac = DatabaseFactory.GetFac();
            var dbTools = dbFac.GetDatabase(DatabaseSelector.XML);
            var db = SchedulerDatabase.GetDb();
            db = dbTools.GetData();
            IUpdateDb updateDb;
            while (true)
            {
                Console.WriteLine("-View Jobs, -Delete or -Add?");
                string entry = Console.ReadLine();
                db = dbTools.GetData();
                if (entry.Equals("view", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var item in db.Configuration.Jobs) Console.WriteLine(item.ToString());
                }
                else if (entry.Equals("add", StringComparison.OrdinalIgnoreCase))
                {
                    updateDb = new AddData(db);
                }
                else if (entry.Equals("delete", StringComparison.OrdinalIgnoreCase))
                {
                    updateDb = new DeleteData(db);
                }
                else Console.WriteLine("Enter a valid command");
            }
        }
    }
}