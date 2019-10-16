using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderLibrary
{
    public class Director
    {
        public void Construct(AbstractBuilder builder, string arg)
        {
            builder.BuildPart(arg);
        }
    }
}
