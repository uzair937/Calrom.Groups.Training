using System;
using FactoryLibrary;
using JobLibrary;

namespace JobConfiguration
{
    class DeleteData : IUpdateDb
    {
        public DeleteData(SchedulerDatabase db)
        {
            UpdateDb(db);
        }
        public void UpdateDb(SchedulerDatabase db)
        {
            var dbFac = DatabaseFactory.GetFac();
            var dbTools = dbFac.GetDatabase(DatabaseSelector.XML);
            Console.WriteLine("Remove -Job or -Email?");
            var entry = Console.ReadLine();
            if (entry.Equals("job", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter the JobId of the Job you'd like to remove");
                var tempJobs = db.Configuration.Jobs;
                var tempIds = db.Configuration.Subscriptions[0];
                var ID = Convert.ToInt32(Console.ReadLine());
                tempJobs.RemoveAll(a => a.Id == ID);
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
            dbTools.AddData(db);
        }
    }
}
