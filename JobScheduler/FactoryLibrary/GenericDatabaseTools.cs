using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobLibrary;

namespace FactoryLibrary
{
    public abstract class GenericDatabaseTools
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
