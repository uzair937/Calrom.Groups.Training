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
    public class MainScheduler
    {
        private List<JobRunInfo> JobList = new List<JobRunInfo>();
        private SchedulerDatabase db = new SchedulerDatabase();
        private ManageXml XmlTools = new ManageXml();
        
        private bool exeRunning;
        private int jobCount;

        private static void Main(string[] args)
        {
            var SchedulerTools = new MainScheduler();
            SchedulerTools.ScheduleJobs();
        }

        private void ScheduleJobs()
        {
            do
            {
                db = XmlTools.GetXmlData();
                Thread.Sleep(2000);
                Console.WriteLine(db.Configuration.Jobs.Count + " Jobs");
            } while (db == null || db.Configuration.Jobs.Count < 3);

            var keepTime = new Thread(TimeKeeper);
            var currentProgram = new Thread(() => RunJob(db.Configuration.Jobs[0], 0, (PriorityEnum)1));
            var dbUpdate = new Thread(UpdateLocalDatabase);
            var currentPriority = new PriorityEnum();
            exeRunning = false;

            foreach (var job in db.Configuration.Jobs)
            {
                JobList.Add(new JobRunInfo
                {
                    JobTime = XmlConvert.ToTimeSpan(job.Interval),
                    JobQueue = XmlConvert.ToTimeSpan(job.Interval),
                    JobId = job.JobId
                });
                Console.WriteLine(job.Interval);
            }

            jobCount = db.Configuration.Jobs.Count;
            keepTime.Start();
            foreach (var item in db.Configuration.Jobs) Console.WriteLine(item.ToString());
            dbUpdate.Start();

            while (true)
            {
                if (db.Configuration.Jobs.Count != jobCount) ManageJobs();
                try
                {
                    for (int i = 0; i < jobCount; i++)
                    {
                        int currentId = JobList[i].JobId;
                        if (JobList[i].JobQueue <= TimeSpan.Zero && ((db.Configuration.Jobs[i].Priority > currentPriority && exeRunning) || !exeRunning))
                        {
                            var currentJob = db.Configuration.Jobs[i];
                            if (currentProgram.IsAlive) currentProgram.Abort();
                            currentPriority = currentJob.Priority;
                            currentProgram = new Thread(() => RunJob(currentJob, currentId, currentPriority));
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

        private void UpdateLocalDatabase()
        {
            while (true)
            {
                db = XmlTools.GetXmlData();
                Thread.Sleep(5000);
            }
        }

        private void ManageJobs()
        {
            if (jobCount < db.Configuration.Jobs.Count)
            {
                var newId = db.Configuration.Jobs.Select(a => a.JobId)
                                .Except(JobList.Select(a => a.JobId))
                                .FirstOrDefault();
                JobList.Add(new JobRunInfo
                {
                    JobQueue = XmlConvert.ToTimeSpan(db.Configuration.Jobs.First(a => a.JobId == newId).Interval),
                    JobTime = XmlConvert.ToTimeSpan(db.Configuration.Jobs.First(a => a.JobId == newId).Interval),
                    JobId = newId
                });
            }
            else
            {
                var oldId = JobList.Select(a => a.JobId)
                                .Except(db.Configuration.Jobs.Select(a => a.JobId))
                                .FirstOrDefault();
                JobList.Remove(JobList.First(a => a.JobId == oldId));
              
            }
            JobList = JobList.OrderBy(a => a.JobId).ToList();
            Console.WriteLine("Database Appended");
            jobCount = db.Configuration.Jobs.Count;
        }

        private void RunJob(Job job, int id, PriorityEnum priority)
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
                //    Console.WriteLine(e.Message);
                //}
                JobList[JobList.FindIndex(a => a.JobId == id)].JobQueue = JobList[JobList.FindIndex(a => a.JobId == id)].JobTime;
                Console.WriteLine("Priority " + priority + ": Job " + id + " Done!");
                exeRunning = false;
            }
        }

        private void TimeKeeper()
        {
            var sleepTime = JobList.Select(a => a.JobTime).Min();
            var sleepQueue = new TimeSpan();
            while (true)
            {
                try
                {
                    sleepQueue = JobList.Select(a => a.JobQueue).Where(f => f > TimeSpan.Zero).Min();
                    if (sleepTime < sleepQueue)
                    {
                        Console.WriteLine("Sleeping for " + sleepTime);
                        Thread.Sleep(sleepTime);
                    }
                    else
                    {
                        Console.WriteLine("Sleeping for " + sleepQueue);
                        Thread.Sleep(sleepQueue);
                    }
                }
                catch
                {
                    Console.WriteLine("Sleep Failed");
                    db = XmlTools.GetXmlData();
                    if (jobCount != db.Configuration.Jobs.Count) ManageJobs();
                }
                for (int t = 0; t < jobCount; t++)
                {
                    try
                    {
                        if (sleepTime > sleepQueue)
                        {
                            if (JobList[t].JobQueue > TimeSpan.Zero) JobList[t].JobQueue -= sleepQueue;
                        }
                        else if (JobList[t].JobQueue > TimeSpan.Zero)
                        {
                            JobList[t].JobQueue -= sleepTime;
                        }
                        Console.WriteLine("Job " + JobList[t].JobId + " has " + JobList[t].JobQueue + " left.");
                    }
                    catch
                    {
                        Console.WriteLine("Append Queue Failed");
                        t--;
                        db = XmlTools.GetXmlData();
                        if (jobCount != db.Configuration.Jobs.Count) ManageJobs();
                    }
                }
                Console.WriteLine("Tick");
            }
        }
    }
}
