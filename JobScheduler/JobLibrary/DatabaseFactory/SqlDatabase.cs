using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace JobLibrary
{
    public class SqlDatabase : GenericDatabase
    {
        public override bool AddData(SchedulerDatabase db)
        {
            try
            {
                using (var SqlDb = new SqlContext())
                {
                    foreach (var job in db.Configuration.Jobs)
                    {
                        var JobAdded = SqlDb.JobDb.FirstOrDefault(f => f.JobId == job.JobId);
                        if (JobAdded == null) SqlDb.JobDb.Add(job);
                    }
                    SqlDb.SaveChanges();
                    foreach (var sub in db.Configuration.Subscriptions)
                    {
                        var JobAdded = SqlDb.EmailDb.FirstOrDefault(f => f.EmailAddress == sub.EmailAddress);
                        if (JobAdded == null) SqlDb.EmailDb.Add(sub);
                    }
                    SqlDb.SaveChanges();
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
            try
            {
                var db = SchedulerDatabase.GetDb();
                using (var SqlDb = new SqlContext())
                {
                    foreach (var job in SqlDb.JobDb)
                    {
                        db.Configuration.Jobs.Add(job);
                    }
                    foreach (var sub in SqlDb.EmailDb)
                    {
                        db.Configuration.Subscriptions.Add(sub);
                    }
                }
                return db;
            }
            catch
            {
                return null;
            }
        }
    }
}
