using CustomRegionEditor.Models;
using System.Collections.Generic;

namespace CustomRegionEditor.Handler.Interfaces
{
    public interface ICustomRegionManager
    {
        ManagerResult<CustomRegionGroupModel> Add(CustomRegionGroupModel customRegionGroupModel);

        bool DeleteById(string id);

        RegionListModel GetRegionList();

        ManagerResult<CustomRegionGroupModel> RemoveRegions(CustomRegionGroupModel customRegionGroupModel, CustomRegionEntryModel customRegionEntryModel);
    }
}
