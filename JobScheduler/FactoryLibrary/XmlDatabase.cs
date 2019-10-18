using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using JobLibrary;

namespace FactoryLibrary
{
    public class XmlDatabase : GenericDatabaseTools
    {
        private const string xmlFileOut = "C:/Users/wbooth/source/repos/JobScheduler/Resources/db-out.xml";
        private readonly object threadLock = new object();

        public override bool AddData(List<IEntity>[] newData)
        {
            try
            {
                using (FileStream fs = new FileStream(xmlFileOut, FileMode.Create))
                {
                    foreach (var data in newData)
                    {
                        var serializer = new XmlSerializer(typeof(List<IEntity>));
                        var sortedData = data.OrderBy(a => a.Id).ToList();
                        serializer.Serialize(fs, sortedData);
                        fs.Close();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override List<IEntity>[] GetData()
        {
            lock (threadLock)
            {
                List<IEntity>[] jobDatabase;
                do
                {
                    try
                    {
                        var serializer = new XmlSerializer(typeof(List<IEntity>[]));
                        using (FileStream fs = new FileStream(xmlFileOut, FileMode.Open))
                        {
                            jobDatabase = (List<IEntity>[])serializer.Deserialize(fs);
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