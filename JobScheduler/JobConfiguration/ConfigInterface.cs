using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using JobLibrary;

namespace JobConfiguration
{
    class ConfigInterface : ManageXml
    {
        private const string xmlFile = "C:/Users/wbooth/Documents/JobTask/db-initial.xml";
        private const string xmlFileOut = "C:/Users/wbooth/Documents/JobTask/db-out.xml";
        private static SchedulerDatabase db = new SchedulerDatabase();

        static void Main(string[] args)
        {
            XmlSetup();
            while (true)
            {
                Console.WriteLine("-View Jobs, -Delete or -Add?");
                string entry = Console.ReadLine();
                db = GetXmlData();
                ManageJson.AddJson(db);
                SQLManager.SqlSetup(db);
                if (entry.ToUpper() == "VIEW" || entry.ToUpper() == "-VIEW") PrintJob();
                if (entry.ToUpper() == "ADD" || entry.ToUpper() == "-ADD") AddData();
                if (entry.ToUpper() == "DELETE" || entry.ToUpper() == "-DELETE") DeleteData();
                AddXml(db);
            }
        }
        private static void PrintJob()
        {
            foreach (var item in db.Configuration.Jobs)
            {
                Console.WriteLine(item.JobId);
                Console.WriteLine(item.Interval);
                Console.WriteLine(item.Enabled);
                Console.WriteLine(item.JobType);
                Console.WriteLine(item.Path);
                Console.WriteLine(item.Arguments);
                Console.WriteLine(item.DateCreated);
                Console.WriteLine(item.Priority);
            }
        }
        private static void AddData()
        {
            Console.WriteLine("Type -Job to add a Job or type -Email to add an EmailSubscription");
            string command = Console.ReadLine();
            if (command.ToUpper() == "JOB" || command.ToUpper() == "-JOB") AddJob();
            else if (command.ToUpper() == "EMAIL" || command.ToUpper() == "-EMAIL") AddEmail();
            else Console.WriteLine("Enter a valid command");
        }
        private static void DeleteData()
        {
            Console.WriteLine("Remove -Job or -Email?");
            string entry = Console.ReadLine();
            if (entry.ToUpper() == "JOB" || entry.ToUpper() == "-JOB")
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
            else if (entry.ToUpper() == "EMAIL" || entry.ToUpper() == "-EMAIL")
            {
                List<EmailSubscription> tempEmail = db.Configuration.Subscriptions;
                Console.WriteLine("Enter the Email you'd like to remove");
                string Email = Console.ReadLine();
                tempEmail.RemoveAll(a => a.EmailAddress == Email);
                db.Configuration.Subscriptions = tempEmail;
            }
        }
        private static void AddEmail()
        {
            Console.WriteLine("Enter new user email");
            var entry = Console.ReadLine();
            int EmailID = -1;
            bool idUsed;
            do
            {
                EmailID++;
                idUsed = false;
                foreach (var id in db.Configuration.Subscriptions)
                {
                    if (id.ID == EmailID) idUsed = true;
                }
            } while (idUsed);
            var tempEmail = new EmailSubscription
            {
                ID = EmailID,
                EmailAddress = entry
            };
            db.Configuration.Subscriptions.Add(tempEmail);
            db.Configuration.Subscriptions[0].JobIds.Add(db.Configuration.Subscriptions[0].JobIds.Max() + 1);
        }
        private static void AddJob()
        {
            Job tempJob = new Job();
            int jobNum = -1;
            bool idUsed;
            do
            {
                jobNum++;
                idUsed = false;
                foreach (var id in db.Configuration.Subscriptions[0].JobIds)
                {
                    if (id == jobNum) idUsed = true;
                }
            } while (idUsed);
            Console.WriteLine($"This will be JobId " + jobNum + ", enter re to cancel");
            tempJob.JobId = jobNum;
            tempJob.DateCreated = DateTime.Now;
            Console.WriteLine("Job Interval?");
            string entry = Console.ReadLine();
            if (entry.ToUpper() == "RE") return;
            tempJob.Interval = entry;
            Console.WriteLine("Enabled at start -Y/N");             //Split interface and xml logic into new classes, pass the strings across
            entry = Console.ReadLine();
            if (entry.ToUpper() == "RE") return;                //interface in jobconfig, xml in lib
            if (entry.ToUpper() == "Y") tempJob.Enabled = true;
            else tempJob.Enabled = false;
            Console.WriteLine("Job Type");
            entry = Console.ReadLine();
            if (entry.ToUpper() == "RE") return;
            tempJob.JobType = entry;
            Console.WriteLine("File Path");
            entry = Console.ReadLine();
            if (entry.ToUpper() == "RE") return;
            tempJob.Path = entry;
            Console.WriteLine("Arguments");
            entry = Console.ReadLine();
            if (entry.ToUpper() == "RE") return;
            tempJob.Arguments = entry;
            Console.WriteLine("Priority -High, -Medium, -Low");
            entry = Console.ReadLine();
            if (entry.ToUpper() == "RE") return;
            tempJob.Priority = (PriorityEnum)Enum.Parse(typeof(PriorityEnum), entry, true);
            db.Configuration.Jobs.Add(tempJob);
            db.Configuration.Subscriptions[0].JobIds.Add(jobNum);
        }
    }
}