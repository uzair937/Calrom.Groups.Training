using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public class ValidEntryModel
    {
        public ErrorModel Error { get; set; }
        
        public CustomRegionEntryModel CustomRegionEntryModel { get; set; }

        public bool ValidEntry { get; set; }
    }
}
