using System.Collections.Generic;
using CustomRegionEditor.Models;

namespace CustomRegionEditor.Handler.Interfaces
{
    public interface IRegionHandler
    {
        List<List<string>> GetRegionList();

        CustomRegionGroupModel GetSubRegions(string searchTerm, string filter);

        bool DeleteById(string id);
    }
}
