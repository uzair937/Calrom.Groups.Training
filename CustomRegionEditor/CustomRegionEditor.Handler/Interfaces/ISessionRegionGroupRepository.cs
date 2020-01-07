using CustomRegionEditor.Models;
using System.Collections.Generic;

namespace CustomRegionEditor.Handler.Interfaces
{
    public interface ISessionRegionGroupRepository
    {
        List<string> AddByType(string entry, string type);
        CustomRegionGroupModel GetSessionRegion();
        void SetSessionRegion(CustomRegionGroupModel customRegionGroupModel);
        void SetDetails(string name, string description);
        bool ValidName(string name);
        CustomRegionGroupModel SaveToDatabase(CustomRegionGroupModel customRegionGroupModel);
        void ClearSession();
    }
}
