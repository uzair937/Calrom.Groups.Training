using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Models
{
    public class ValidationModel
    {
        public List<ErrorModel> Errors { get; set; }
        
        public CustomRegionGroupModel CustomRegionGroupModel { get; set; }

        public ValidationModel()
        {
            Errors = new List<ErrorModel>();
        }
    }
}
