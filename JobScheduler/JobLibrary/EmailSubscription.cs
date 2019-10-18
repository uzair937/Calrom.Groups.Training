using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobLibrary
{
    public class EmailSubscription
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string LogLevel { get; set; }
        public List<int> JobIds { get; set; }
    }
}
