using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobLibrary
{
    public class Config
    {
        public List<Job> Jobs { get; set; }
        public List<EmailSubscription> Subscriptions { get; set; }
    }
}
