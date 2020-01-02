using CustomRegionEditor.Database.Models;
using System.Collections.Generic;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface ICustomRegionGroupRepository : IRepository<CustomRegionGroupModel>
    {
        List<string> GetNames(string type);
        List<CustomRegionGroupModel> GetSearchResults(string searchTerm, string filter);
        CustomRegionGroupModel AddByType(string entry, string type, string regionId);
        void ChangeDetails(string name, string description, string id);
        CustomRegionGroupModel AddNewRegion(string name, string description);
        void UpdateList(IList<CustomRegionEntryModel> list, string regionId);
    }
}
