using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduler
{
    public class JobRunInfo
    {
        public TimeSpan JobQueue { get; set; }
        public TimeSpan JobTime { get; set; }
        public int JobId { get; set; }

        public JobRunInfo SetQueueTime(TimeSpan time)
        {
            JobQueue = time;
            JobTime = time;
            return this;
        }

        public JobRunInfo SetId(int id)
        {
            JobId = id;
            return this;
        }

        public JobRunInfo Combine(JobRunInfo PartA, JobRunInfo PartB)
        {
            if (PartA.JobId == 0)
            {
                PartA.JobId = PartB.JobId;
                return PartA;
            }
            else
            {
                PartB.JobId = PartA.JobId;
                return PartB;
            }
        }
    }
}
