using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobLibrary;

namespace JobConfiguration
{
    interface IUpdateDb
    {
        void UpdateDb(SchedulerDatabase db);
    }
}
