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
        private JobRunInfo Product = new JobRunInfo();
        public override void BuildPartA(string arg)
        {
            Product.SetId(int.Parse(arg));
        }
        public override void BuildPartB(string arg)
        {
            Product.SetId(0);
            Product.SetQueueTime(XmlConvert.ToTimeSpan(arg));
        }
        public override JobRunInfo GetResult()
        {
            return Product;
        }
    }
}
