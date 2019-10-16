using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobScheduler;

namespace BuilderLibrary
{
    public class ConcreteBuilderId : AbstractBuilder
    {
        private JobRunInfo PartTwo = new JobRunInfo();
        public override void BuildPart(string arg)
        {
            PartTwo.SetId(int.Parse(arg));
        }
        public override JobRunInfo GetResult()
        {
            return PartTwo;
        }
    }
}
