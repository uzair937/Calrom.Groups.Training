using CustomRegionEditor.Models;
using System.Collections.Generic;

namespace CustomRegionEditor.Handler.Interfaces
{
    public interface ICustomRegionManager
    {
        ValidationModel Add(CustomRegionGroupModel customRegionGroupModel);

        bool DeleteById(string id);

        RegionListModel GetRegionList();
    }
}
