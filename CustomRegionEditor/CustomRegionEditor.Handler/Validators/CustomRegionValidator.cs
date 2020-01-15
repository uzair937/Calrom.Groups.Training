using CustomRegionEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Handler.Validators
{
    public class CustomRegionValidator
    {
        public bool IsValid(CustomRegionGroupModel customRegion)
        {
            var superRegions = new CustomRegionEntrySupersetValidator().IsValid(customRegion);
            var validEntries = new CustomRegionEntrySupersetValidator().IsValid(customRegion);

            return (superRegions && validEntries);
        }
    }
}
