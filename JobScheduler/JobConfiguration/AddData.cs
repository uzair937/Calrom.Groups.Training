using System;
using System.Linq;
using FactoryLibrary;
using JobLibrary;

namespace JobConfiguration
{
    public class AddData : IUpdateDb
    {
        public AddData(SchedulerDatabase db, GenericDatabaseTools dbTools)
        {
            UpdateDb(db, dbTools);
        }
        public void UpdateDb(SchedulerDatabase db, GenericDatabaseTools dbTools)
        {
            Console.WriteLine("Type -Job to add a Job or type -Email to add an EmailSubscription");
            var command = Console.ReadLine();
            if (command.Equals("job", StringComparison.OrdinalIgnoreCase)) AddJob(db);
            else if (command.Equals("email", StringComparison.OrdinalIgnoreCase)) AddEmail(db);
            else Console.WriteLine("Enter a valid command");
            dbTools.AddData(db);
        }
        private void AddEmail(SchedulerDatabase db)
        {
            Console.WriteLine("Enter new user email");
            var entry = Console.ReadLine();
            var EmailID = Enumerable.Range(1, int.MaxValue)
                                .Except(db.Configuration.Subscriptions.Select(u => u.Id))
                                .FirstOrDefault();
            var tempEmail = new EmailSubscription
            {
                Id = EmailID,
                EmailAddress = entry
            };
            db.Configuration.Subscriptions.Add(tempEmail);
        }

        private void AddJob(SchedulerDatabase db)
        {
            var tempJob = new Job();
            var newJob = new string[7];
            var jobNum = Enumerable.Range(1, int.MaxValue)
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
