using CustomRegionEditor.Database.Models;
using System.Collections.Generic;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface ICustomRegionGroupRepository : IRepository<CustomRegionGroupModel>
    {
        List<string> GetNames(string type);
        CustomRegionGroupModel GetFilteredResults(string countryName, string filter);
        List<CustomRegionGroupModel> GetSearchResults(string searchTerm, string filter);
        void AddByType(string entry, string type, string regionId);
        void ChangeDetails(string name, string description, string regionId);
        CustomRegionGroupModel AddNewRegion();
    }
}
