using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobScheduler;

namespace BuilderLibrary
{
    public abstract class AbstractBuilder
    {
        public abstract void BuildPart(string arg);
        public abstract JobRunInfo GetResult();
    }
}
