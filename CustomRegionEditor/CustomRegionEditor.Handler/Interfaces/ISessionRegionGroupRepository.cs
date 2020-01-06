using CustomRegionEditor.Models;

namespace CustomRegionEditor.Handler.Interfaces
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
