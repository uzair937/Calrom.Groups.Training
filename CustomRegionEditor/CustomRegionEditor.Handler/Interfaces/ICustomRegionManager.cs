using CustomRegionEditor.Models;
using System.Collections.Generic;

namespace CustomRegionEditor.Handler.Interfaces
{
    public interface ICustomRegionManager
    {
        CustomRegionGroupModel Add(CustomRegionGroupModel customRegionGroupModel);

        bool DeleteById(string id);

        CustomRegionGroupModel GetSubRegions(string searchTerm, string filter);

        RegionListModel GetRegionList();
    }
}
