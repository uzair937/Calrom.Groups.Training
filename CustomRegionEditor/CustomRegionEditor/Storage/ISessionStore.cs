using CustomRegionEditor.ViewModels;

namespace CustomRegionEditor.Web.Interfaces
{
    public interface ISessionStore
    {
        void Save(CustomRegionGroupViewModel item);

        CustomRegionGroupViewModel Get();

        void Clear();
    }
}
