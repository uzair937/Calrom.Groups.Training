using CustomRegionEditor.Database.Models;
using System.Collections.Generic;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface ICustomRegionGroupRepository : IRepository<CustomRegionGroupModel>
    {
        List<string> GetNames(string type);
        List<CustomRegionGroupModel> GetSearchResults(string searchTerm, string filter);
        CustomRegionGroupModel AddNewRegion(CustomRegionGroupModel customRegionGroupModel);
    }
}
