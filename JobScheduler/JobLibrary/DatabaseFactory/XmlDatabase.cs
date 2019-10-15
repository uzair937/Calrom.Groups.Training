using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Threading;

namespace JobLibrary
{
    public class XmlDatabase : GenericDatabase
    {
        private const string xmlFileOut = "C:/Users/wbooth/source/repos/JobScheduler/Resources/db-out.xml";
        private readonly object threadLock = new object();

        public override bool AddData(SchedulerDatabase newData)
        {
            try
            {
                using (FileStream fs = new FileStream(xmlFileOut, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(SchedulerDatabase));
                    var sortedData = newData.Configuration.Jobs.OrderBy(job => job.JobId).ToList();
                    newData.Configuration.Jobs = sortedData;
                    serializer.Serialize(fs, newData);
                    fs.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override SchedulerDatabase GetData()
        {
            lock (threadLock)
            {
                var jobDatabase = SchedulerDatabase.GetDb();
                do
                {
                    try
                    {
                        var serializer = new XmlSerializer(typeof(SchedulerDatabase));
                        using (FileStream fs = new FileStream(xmlFileOut, FileMode.Open))
                        {
                            jobDatabase = (SchedulerDatabase)serializer.Deserialize(fs);
                            fs.Close();
                        }
                    }
                    catch
                    {
                        Thread.Sleep(100);
                        jobDatabase = null;
                    }
                } while (jobDatabase == null);
                return jobDatabase;
            }
        }
    }
}