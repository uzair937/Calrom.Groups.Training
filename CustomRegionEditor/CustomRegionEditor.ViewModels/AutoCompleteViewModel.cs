using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.ViewModels
{
    public class AutoCompleteViewModel
    {
        public List<string> Suggestions { get; set; }

        public AutoCompleteViewModel()
        {
            Suggestions = new List<string>();
        }
    }
}
