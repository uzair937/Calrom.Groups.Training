﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobLibrary
{
    public class SchedulerDatabase
    {
        public Config Configuration { get; set; }
        public SchedulerDatabase()
        {
            Configuration = new Config();
        }
    }
}
