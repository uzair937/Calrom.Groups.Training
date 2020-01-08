using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public class ErrorModel
    {
        public string FailedToAdd { get; set; }
        public string RemovedChildren { get; set; }
        public string FailedToDelete { get; set; }
        public bool AddFailed { get; set; }
        public bool RegionsRemoved { get; set; }
        public bool SingleRegionRemoved { get; set; }
        public bool DeleteFailed { get; set; }

        public ErrorModel()
        {
            FailedToAdd = string.Empty;
            RemovedChildren = string.Empty;
            FailedToDelete = string.Empty;
            AddFailed = false;
            RegionsRemoved = false;
            SingleRegionRemoved = false;
            DeleteFailed = false;
        }
    }
}
