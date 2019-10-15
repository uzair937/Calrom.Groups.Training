using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobLibrary
{
    public class SchedulerDatabase
    {
        private static SchedulerDatabase obj;

        private SchedulerDatabase() { }

        public static SchedulerDatabase GetDb()
        {
            if (obj == null) obj = new SchedulerDatabase();
            return obj;
        }

        public Config Configuration { get; set; }
    }
}
