using CustomRegionEditor.Database.Models;
using System.Collections.Generic;

namespace CustomRegionEditor.Database
{
    public interface ICustomRegionGroupRepository : IRepository<CustomRegionGroupModel>
    {
        List<string> GetNames(string type);
        List<CustomRegionGroupModel> GetFilteredResults(string countryName, string filter);
        List<CustomRegionGroupModel> GetSearchResults(string searchTerm, string filter);
        void DeleteById(string id);
        void DeleteEntry(string entryId, string regionId);
        void AddByType(string entry, string type, string regionId);
        void ChangeDetails(string name, string description, string regionId);
        CustomRegionGroupModel AddNewRegion();
        CustomRegionGroupModel FindById(string id);
    }
}
