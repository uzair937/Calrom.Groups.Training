using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using JobLibrary;

namespace JobScheduler
{
    class MainScheduler
    {
        private static SchedulerDatabase db = new SchedulerDatabase();
        private static List<TimeSpan> JobQueue = new List<TimeSpan>();
        private static List<TimeSpan> JobTime = new List<TimeSpan>();
        private static List<int> JobIdList = new List<int>();  
        private static bool exeRunning;
        private static int jobCount;
        static void Main(string[] args)
        {
            do {
                db = ManageXml.GetXmlData();
                Thread.Sleep(2000);
                Console.WriteLine(db.Configuration.Jobs.Count);
            } while (db == null || db.Configuration.Jobs.Count < 3);

            var keepTime = new Thread(TimeKeeper);
            var currentProgram = new Thread(() => RunJob(db.Configuration.Jobs[0], 0, (PriorityEnum)1));
            var dbUpdate = new Thread(UpdateLocalDatabase);
            var currentPriority = new PriorityEnum();
            exeRunning = false;
            foreach (var job in db.Configuration.Jobs)
            {
                JobTime.Add(XmlConvert.ToTimeSpan(job.Interval));
                JobQueue.Add(XmlConvert.ToTimeSpan(job.Interval));
                JobIdList.Add(job.JobId);
                Console.WriteLine(job.Interval);
            }
            jobCount = db.Configuration.Jobs.Count;
            keepTime.Start();
            PrintJob();
            dbUpdate.Start();

            while (true)
            {
                if (db.Configuration.Jobs.Count != jobCount) ManageJobs();
                try
                {
                    for (int i = 0; i < jobCount; i++)
                    {
                        int currentIndex = i;
                        if (JobQueue[i] <= TimeSpan.Zero && ((db.Configuration.Jobs[i].Priority > currentPriority && exeRunning) || !exeRunning))
                        {
                            var currentJob = db.Configuration.Jobs[i];
                            if (currentProgram.IsAlive) currentProgram.Abort();
                            currentPriority = currentJob.Priority;
                            currentProgram = new Thread(() => RunJob(currentJob, currentIndex, currentPriority));
                            currentProgram.Start();
                            
                        }
                    }
                }
                catch
                {
                    while (jobCount != db.Configuration.Jobs.Count) Thread.Sleep(100);
                }
            }
        }
        private static void UpdateLocalDatabase()
        {
            while (true)
            {
                db = ManageXml.GetXmlData();
                Thread.Sleep(5000);
            }
        }
        private static void ManageJobs()
        {
            if (jobCount < db.Configuration.Jobs.Count)
            {
                for (int i = 0; i < db.Configuration.Jobs.Count - jobCount; i++)
                {
                    JobTime.Add(XmlConvert.ToTimeSpan(db.Configuration.Jobs[jobCount + i].Interval));
                    JobQueue.Add(XmlConvert.ToTimeSpan(db.Configuration.Jobs[jobCount + i].Interval));
                    JobIdList.Add(db.Configuration.Jobs[jobCount + i].JobId);
                }
            }
            else
            {
                bool remove = false;
                var newIdList = db.Configuration.Subscriptions[0].JobIds;
                do
                {
                    int i = 0;
                    foreach (var oldId in JobIdList)
                    {
                        remove = true;
                        foreach (var newId in newIdList)
                        {
                            if (oldId == newId)
                            {
                                remove = false;
                            }
                        }
                        if (remove)
                        {
                            JobTime.RemoveAt(i);
                            JobQueue.RemoveAt(i);
                            JobIdList.RemoveAt(i);
                            db = ManageXml.GetXmlData();
                            break;
                        }
                        i++;
                    }
                } while (remove);
            }
            Console.WriteLine("Database Appended");
            jobCount = db.Configuration.Jobs.Count;
        }
        private static void RunJob(Job job, int t, PriorityEnum priority)
        {
            if (job.Enabled)
            {
                exeRunning = true;
                Thread.Sleep(3000);
                //ProcessStartInfo startInfo = new ProcessStartInfo(job.Path)
                //{
                //    CreateNoWindow = true,
                //    UseShellExecute = false,
                //    FileName = job.JobType,
                //    WindowStyle = ProcessWindowStyle.Hidden,
                //    Arguments = job.Arguments
                //};
                //try
                //{
                //    Process.Start(startInfo);
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e);
                //}
                JobQueue[t] = JobTime[t];
                Console.WriteLine("Priority " + priority + ": Job " + JobIdList[t] + " Done!");
                exeRunning = false;
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
        private static void TimeKeeper()
        {
            while (true)
            {
                try
                {
                    if (JobTime.Min() < JobQueue.Where(f => f > TimeSpan.Zero).Min())
                    {
                        Console.WriteLine("Sleeping for " + JobTime.Min());
                        Thread.Sleep(JobTime.Min());
                    }
                    else
                    {
                        Console.WriteLine("Sleeping for " + JobQueue.Where(f => f > TimeSpan.Zero).Min());
                        Thread.Sleep(JobQueue.Where(f => f > TimeSpan.Zero).Min());
                    }
                }
                catch
                {
                    Console.WriteLine("Sleep Failed");
                    db = ManageXml.GetXmlData();
                    if (jobCount != db.Configuration.Jobs.Count) ManageJobs();
                }
                for (int t = 0; t < jobCount; t++)
                {
                    try
                    {
                        if (JobTime.Min() > JobQueue.Where(f => f > TimeSpan.Zero).Min())
                        {
                            if (JobQueue[t] > TimeSpan.Zero) JobQueue[t] -= JobQueue.Where(f => f > TimeSpan.Zero).Min();
                        }
                        else if (JobQueue[t] > TimeSpan.Zero)
                        {
                            JobQueue[t] -= JobTime.Min();
                        }
                        Console.WriteLine("Job " + JobIdList[t] + " has " + JobQueue[t] + " left.");
                    }
                    catch
                    {
                        Console.WriteLine("Append Queue Failed");
                        t--;
                        db = ManageXml.GetXmlData();
                        if (jobCount != db.Configuration.Jobs.Count) ManageJobs();
                    }
                }
                Console.WriteLine("Tick");
            }
        }
    }
}
