using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobLibrary
{
    public abstract class GenericDatabase
    {
        public abstract bool AddData(SchedulerDatabase db);
        public abstract SchedulerDatabase GetData();
    }

    public enum DatabaseSelector
    {
        JSON,
        XML,
        SQL
    }
}
