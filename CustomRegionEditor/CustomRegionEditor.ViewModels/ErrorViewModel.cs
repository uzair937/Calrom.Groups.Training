using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.ViewModels
{
    public class ErrorViewModel
    {
        public string Message { get; set; }
        public bool Warning { get; set; }

        public ErrorViewModel()
        {
            Message = string.Empty;
            Warning = false;
        }
    }
}
