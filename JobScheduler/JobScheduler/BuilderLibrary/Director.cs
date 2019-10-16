using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduler
{
    public class Director
    {
        public void Construct(AbstractBuilder builder, string[] args)
        {
            builder.BuildPartA(args[0]);
            builder.BuildPartB(args[1]);
        }
    }
}
