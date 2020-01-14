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
        public bool IsValid(CustomRegionGroupModel customRegionGroupModel)
        {
            if (customRegionGroupModel.CustomRegionEntries == null || customRegionGroupModel.CustomRegionEntries.Count == 0)
            {
                return false;
            }

            foreach (var customRegionEntryModel in customRegionGroupModel.CustomRegionEntries)
            {
                var valid = this.IsEntryValid(customRegionGroupModel, customRegionEntryModel);

                if (!valid)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsEntryValid(CustomRegionGroupModel customRegionGroupModel, CustomRegionEntryModel customRegionEntryModel)
        {
            var type = customRegionEntryModel.GetLocationType();

            if (type == "airport" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.Airport?.Name == customRegionEntryModel?.Airport?.Name) == null)
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
            if (type == "city" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.City?.Name == customRegionEntryModel?.City?.Name) == null)
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
            if (type == "state" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.State?.Name == customRegionEntryModel?.State?.Name) == null)
            {
                if ((!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.Name).Contains(customRegionEntryModel.State?.Country?.Name) || customRegionEntryModel.State?.Country?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.Name).Contains(customRegionEntryModel.State?.Country?.Region?.Name) || customRegionEntryModel.State?.Country?.Region?.Name == null))
                {
                    return true;
                }
            }
            if (type == "country" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.Country?.Name == customRegionEntryModel?.Country?.Name) == null)
            {
                if (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.Name).Contains(customRegionEntryModel.Country?.Region?.Name) || customRegionEntryModel.Country?.Region?.Name == null)
                {
                    return true;
                }
            }
            if (type == "region" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.Region?.Name == customRegionEntryModel?.Region?.Name) == null)
            {
                return true;
            }

            return false;
        }
    }
}
