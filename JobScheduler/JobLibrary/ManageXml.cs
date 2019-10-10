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
    public class ManageXml
    {
        //private const string xmlFile = "C:/Users/wbooth/Documents/JobTask/db-initial.xml";
        private const string xmlFileOut = "C:/Users/wbooth/Documents/JobTask/db-out.xml";

        public static void AddXml(SchedulerDatabase newData)
        {
            using (FileStream fs = new FileStream(xmlFileOut, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(SchedulerDatabase));
                newData.Configuration.Jobs = newData.Configuration.Jobs.OrderBy(job => job.JobId).ToList();
                serializer.Serialize(fs, newData);
                fs.Close();
            }
        }
        public static SchedulerDatabase GetXmlData()
        {
            var jobDatabase = new SchedulerDatabase();
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