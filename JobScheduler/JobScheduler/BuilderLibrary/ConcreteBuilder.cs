using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JobScheduler
{
    public class ConcreteBuilderId : AbstractBuilder
    {
        private JobRunInfo Product;
        public override void BuildPartA(string arg)
        {
            Product = new JobRunInfo();
            Product.SetId(int.Parse(arg));
        }
        public override void BuildPartB(string arg)
        {
            Product.SetQueueTime(XmlConvert.ToTimeSpan(arg));
        }
        public override JobRunInfo GetResult()
        {
            return Product;
        }
    }
}
