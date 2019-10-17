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
    }
}
