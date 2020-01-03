using CustomRegionEditor.Database.Models;
using System.Collections.Generic;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface ISessionRegionGroupRepository
    {
        void AddByType(string entry, string type);
        CustomRegionGroupModel GetSessionRegion();
        void SetSessionRegion(CustomRegionGroupModel customRegionGroupModel);
        CustomRegionGroupModel SaveToDatabase(CustomRegionGroupModel customRegionGroupModel);
        void ClearSession();
    }
}
