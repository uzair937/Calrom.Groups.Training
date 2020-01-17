using CustomRegionEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Handler.Validators
{
    public class CustomRegionEntrySupersetValidator
    {
        public ValidationModel IsValid(CustomRegionGroupModel customRegionGroupModel)
        {
            var validationModel = new ValidationModel();

            if (customRegionGroupModel.CustomRegionEntries == null || customRegionGroupModel.CustomRegionEntries.Count == 0)
            {
                var error = new ErrorModel
                {
                    Message = "No Entries in Custom Region",
                    Warning = true
                };
                validationModel.Errors.Add(error);
                return validationModel;
            }

            foreach (var customRegionEntryModel in customRegionGroupModel.CustomRegionEntries)
            {
                var valid = this.IsEntryValid(customRegionGroupModel, customRegionEntryModel);
                
                if (!valid)
                {
                    var error = new ErrorModel
                    {
                        Message = "Entry " + customRegionEntryModel.LocationName + " Is Invalid",
                        Warning = true
                    };
                    validationModel.Errors.Add(error);
                }
            }

            return validationModel;
        }

        private bool IsEntryValid(CustomRegionGroupModel customRegionGroupModel, CustomRegionEntryModel customRegionEntryModel)
        {
            var type = customRegionEntryModel.GetLocationType();

            if (type == "airport")
            {
                if ((!customRegionGroupModel.CustomRegionEntries.Select(c => c.City?.Name).Contains(customRegionEntryModel.Airport?.City?.Name) || customRegionEntryModel.Airport?.City?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.State?.Name).Contains(customRegionEntryModel.Airport?.City?.State?.Name) || customRegionEntryModel.Airport?.City?.State?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.Name).Contains(customRegionEntryModel.Airport?.City?.Country?.Name) || customRegionEntryModel.Airport?.City?.State?.Country?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.Name).Contains(customRegionEntryModel.Airport?.City?.State?.Country?.Name) || customRegionEntryModel.Airport?.City?.State?.Country?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.Name).Contains(customRegionEntryModel.Airport?.City?.State?.Country?.Region?.Name) || customRegionEntryModel.Airport?.City?.State?.Country?.Region?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.Name).Contains(customRegionEntryModel.Airport?.City?.Country?.Region?.Name) || customRegionEntryModel.Airport?.City?.Country?.Region?.Name == null))
                {
                    return true;
                }
            }
            if (type == "city")
            {
                if ((!customRegionGroupModel.CustomRegionEntries.Select(c => c.State?.Name).Contains(customRegionEntryModel.City?.State?.Name) || customRegionEntryModel.City?.State?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.Name).Contains(customRegionEntryModel.City?.Country?.Name) || customRegionEntryModel.City?.Country?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.Name).Contains(customRegionEntryModel.City?.State?.Country?.Name) || customRegionEntryModel.City?.State?.Country?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.Name).Contains(customRegionEntryModel.City?.State?.Country?.Region?.Name) || customRegionEntryModel.City?.State?.Country?.Region?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.Name).Contains(customRegionEntryModel.City?.Country?.Region?.Name) || customRegionEntryModel.City?.Country?.Region?.Name == null))
                {
                    return true;
                }
            }
            if (type == "state")
            {
                if ((!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.Name).Contains(customRegionEntryModel.State?.Country?.Name) || customRegionEntryModel.State?.Country?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.Name).Contains(customRegionEntryModel.State?.Country?.Region?.Name) || customRegionEntryModel.State?.Country?.Region?.Name == null))
                {
                    return true;
                }
            }
            if (type == "country")
            {
                if (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.Name).Contains(customRegionEntryModel.Country?.Region?.Name) || customRegionEntryModel.Country?.Region?.Name == null)
                {
                    return true;
                }
            }
            if (type == "region")
            {
                return true;
            }

            return false;
        }
    }
}
