using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobScheduler;

namespace BuilderLibrary
{
    public class ConcreteBuilderTime : AbstractBuilder
    {
        private JobRunInfo PartOne = new JobRunInfo();
        public override void BuildPart(string time)
        {
            PartOne.SetQueueTime(TimeSpan.Parse(time));
        }
        public override JobRunInfo GetResult()
        {
            return PartOne;
        }
    }
}
