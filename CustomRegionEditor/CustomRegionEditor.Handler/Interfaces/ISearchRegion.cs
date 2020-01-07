using System.Collections.Generic;
using CustomRegionEditor.Models;

namespace CustomRegionEditor.Handler.Interfaces
{
    public interface ISearchRegion
    {
        List<CustomRegionGroupModel> GetSearchResults(string searchTerm, string filter);

        CustomRegionGroupModel FindById(string id);

        bool CheckValidName(string name);
    }
}
