using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace JobLibrary
{
    public class SQLManager
    {
        public static void SqlSetup(SchedulerDatabase db)
        {
            using (var SqlDb = new SQLContext())
            {
                foreach (var job in db.Configuration.Jobs)
                {
                    var JobAdded = SqlDb.JobDb.Where(f => f.JobId == job.JobId).FirstOrDefault();
                    if (JobAdded == null) SqlDb.JobDb.Add(job);
                }
                SqlDb.SaveChanges();
                foreach (var sub in db.Configuration.Subscriptions)
                {
                    var JobAdded = SqlDb.EmailDb.Where(f => f.EmailAddress == sub.EmailAddress).FirstOrDefault();
                    if (JobAdded == null) SqlDb.EmailDb.Add(sub);
                }
                SqlDb.SaveChanges();
            }
            
        }
    }
}
