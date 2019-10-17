using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JobLibrary
{
    public partial class Job : IEntity
    {
        public int JobId { get; set; }
        public string Interval { get; set; }
        public bool Enabled { get; set; }
        public string JobType { get; set; }
        public string Path { get; set; }
        public string Arguments { get; set; }
        public DateTime DateCreated { get; set; }
        public PriorityEnum Priority { get; set; }

        public void SetValues(string[] args)
        {
            JobId = int.Parse(args[0]);
            Interval = args[1];
            if (args[2] == "Y") Enabled = true;
            else Enabled = false;
            JobType = args[3];
            Path = args[4];
            Arguments = args[5];
            DateCreated = DateTime.Now;
            Priority = (PriorityEnum)Enum.Parse(typeof(PriorityEnum), args[6], true);
        }

        public override string ToString()
        {
            return "---JobId: " + JobId + "\n" + "Interval: " + Interval + "\n" + "Enabled: " + Enabled + "\n" + "JobType: " + JobType + "\n" + "Path: " + Path + "\n" + "Arguments: " + Arguments + "\n" + "DateCreated: " + DateCreated + "\n" + "Priority: " + Priority;
        }

        
    }
    public enum PriorityEnum { Low, Medium, High }
}
