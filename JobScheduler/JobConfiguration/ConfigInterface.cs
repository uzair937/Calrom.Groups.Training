using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
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
            var dbTools = dbFac.GetDatabase(DatabaseSelector.SQL);
            var db = SchedulerDatabase.GetDb();
            var ManageInterface = new ConfigInterface();
            db = dbTools.GetData();
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
                    ManageInterface.AddData(db);
                    dbTools.AddData(db);
                }
                else if (entry.Equals("delete", StringComparison.OrdinalIgnoreCase))
                {
                    ManageInterface.DeleteData(db);
                    dbTools.AddData(db);
                }
                else Console.WriteLine("Enter a valid command");
            }
        }

        private void AddData(SchedulerDatabase db)
        {
            Console.WriteLine("Type -Job to add a Job or type -Email to add an EmailSubscription");
            var command = Console.ReadLine();
            if (command.Equals("job", StringComparison.OrdinalIgnoreCase)) AddJob(db);
            else if (command.Equals("email", StringComparison.OrdinalIgnoreCase)) AddEmail(db);
            else Console.WriteLine("Enter a valid command");
        }

        private void DeleteData(SchedulerDatabase db)
        {
            Console.WriteLine("Remove -Job or -Email?");
            var entry = Console.ReadLine();
            if (entry.Equals("job", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter the JobId of the Job you'd like to remove");
                var tempJobs = db.Configuration.Jobs;
                var tempIds = db.Configuration.Subscriptions[0];
                var ID = Convert.ToInt32(Console.ReadLine());
                tempJobs.RemoveAll(a => a.JobId == ID);
                tempIds.JobIds.RemoveAll(a => a == ID);
                db.Configuration.Jobs = tempJobs;
                db.Configuration.Subscriptions[0] = tempIds;
            }
            else if (entry.Equals("email", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter the Email you'd like to remove");
                var tempEmail = db.Configuration.Subscriptions;
                var Email = Console.ReadLine();
                tempEmail.RemoveAll(a => a.EmailAddress == Email);
                db.Configuration.Subscriptions = tempEmail;
            }
        }

        private void AddEmail(SchedulerDatabase db)
        {
            Console.WriteLine("Enter new user email");
            var entry = Console.ReadLine();
            var EmailID = Enumerable.Range(0, int.MaxValue)
                                .Except(db.Configuration.Subscriptions.Select(u => u.ID))
                                .FirstOrDefault();
            var tempEmail = new EmailSubscription
            {
                ID = EmailID,
                EmailAddress = entry
            };
            db.Configuration.Subscriptions.Add(tempEmail);
        }

        private void AddJob(SchedulerDatabase db)
        {
            var tempJob = new Job();
            var newJob = new string[7];
            var jobNum = Enumerable.Range(0, int.MaxValue)
                                .Except(db.Configuration.Subscriptions[0].JobIds)
                                .FirstOrDefault();
            newJob[0] = jobNum.ToString();
            Console.WriteLine($"This will be JobId " + jobNum + ", enter re to cancel");
            Console.WriteLine("Job Interval?");
            newJob[1] = Console.ReadLine();                         //do validity check
            Console.WriteLine("Enabled at start -Y/N");            
            newJob[2] = Console.ReadLine();
            Console.WriteLine("Job Type");
            newJob[3] = Console.ReadLine();
            Console.WriteLine("File Path");
            newJob[4] = Console.ReadLine();
            Console.WriteLine("Arguments");
            newJob[5] = Console.ReadLine();
            Console.WriteLine("Priority -High, -Medium, -Low");
            newJob[6] = Console.ReadLine();
            tempJob.SetValues(newJob);
            db.Configuration.Jobs.Add(tempJob);
            db.Configuration.Subscriptions[0].JobIds.Add(jobNum);
        }
    }
}