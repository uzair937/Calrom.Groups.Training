using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public class ErrorModel
    {
        public string Message { get; set; }
        public bool Warning { get; set; }

        public ErrorModel()
        {
            Message = string.Empty;
            Warning = false;
        }
    }
}
