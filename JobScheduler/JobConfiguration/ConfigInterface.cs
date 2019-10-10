using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using JobLibrary;

namespace JobConfiguration
{
    class ConfigInterface
    {
        private static SchedulerDatabase db = new SchedulerDatabase();

        static void Main(string[] args)
        {
            var ManageInterface = new ConfigInterface();
            ManageXml.XmlSetup();
            db = ManageXml.GetXmlData();
            ManageJson.AddJson(db);
            SQLManager.SqlSetup(db);
            while (true)
            {
                Console.WriteLine("-View Jobs, -Delete or -Add?");
                string entry = Console.ReadLine();
                db = ManageXml.GetXmlData();
                if (entry.Equals("view", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var item in db.Configuration.Jobs) Console.WriteLine(item.ToString());
                }
                else if (entry.Equals("add", StringComparison.OrdinalIgnoreCase))
                {
                    ManageInterface.AddData();
                    ManageXml.AddXml(db);
                }
                else if (entry.Equals("delete", StringComparison.OrdinalIgnoreCase))
                {
                    ManageInterface.DeleteData();
                    ManageXml.AddXml(db);
                }
                else Console.WriteLine("Enter a valid command");
                
            }
        }

        private void AddData()
        {
            Console.WriteLine("Type -Job to add a Job or type -Email to add an EmailSubscription");
            string command = Console.ReadLine();
            if (command.Equals("job", StringComparison.OrdinalIgnoreCase)) AddJob();
            else if (command.Equals("email", StringComparison.OrdinalIgnoreCase)) AddEmail();
            else Console.WriteLine("Enter a valid command");
        }

        private void DeleteData()
        {
            Console.WriteLine("Remove -Job or -Email?");
            string entry = Console.ReadLine();
            if (entry.Equals("job", StringComparison.OrdinalIgnoreCase))
            {
                List<Job> tempJobs = db.Configuration.Jobs;
                EmailSubscription tempIds = db.Configuration.Subscriptions[0];
                Console.WriteLine("Enter the JobId of the Job you'd like to remove");
                int ID = Convert.ToInt32(Console.ReadLine());
                tempJobs.RemoveAll(a => a.JobId == ID);
                tempIds.JobIds.RemoveAll(a => a == ID);
                db.Configuration.Jobs = tempJobs;
                db.Configuration.Subscriptions[0] = tempIds;
            }
            else if (entry.Equals("email", StringComparison.OrdinalIgnoreCase))
            {
                List<EmailSubscription> tempEmail = db.Configuration.Subscriptions;
                Console.WriteLine("Enter the Email you'd like to remove");
                string Email = Console.ReadLine();
                tempEmail.RemoveAll(a => a.EmailAddress == Email);
                db.Configuration.Subscriptions = tempEmail;
            }
        }

        private void AddEmail()
        {
            Console.WriteLine("Enter new user email");
            var entry = Console.ReadLine();
            int EmailID = Enumerable.Range(0, int.MaxValue)
                                .Except(db.Configuration.Subscriptions.Select(u => u.ID))
                                .FirstOrDefault();
            var tempEmail = new EmailSubscription
            {
                ID = EmailID,
                EmailAddress = entry
            };
            db.Configuration.Subscriptions.Add(tempEmail);
            db.Configuration.Subscriptions[0].JobIds.Add(db.Configuration.Subscriptions[0].JobIds.Max() + 1);
        }

        private void AddJob()
        {
            var tempJob = new Job();
            string[] newJob = new string[7];
            int jobNum = Enumerable.Range(0, int.MaxValue)
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