using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduler
{
    public abstract class AbstractBuilder
    {
        public abstract void BuildPartA(string arg);
        public abstract void BuildPartB(string arg);
        public abstract JobRunInfo GetResult();
    }
}
