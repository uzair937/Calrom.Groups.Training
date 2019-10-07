using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobLibrary
{
    public class Job
    {
        public int JobId { get; set; }
        public string Interval { get; set; }
        public bool Enabled { get; set; }
        public string JobType { get; set; }
        public string Path { get; set; }
        public string Arguments { get; set; }
        public DateTime DateCreated { get; set; }
        public PriorityEnum Priority { get; set; }
    }
    public enum PriorityEnum { Low, Medium, High }
}
