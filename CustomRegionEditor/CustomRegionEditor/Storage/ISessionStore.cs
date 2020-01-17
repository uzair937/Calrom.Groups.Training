using CustomRegionEditor.ViewModels;
using System.Collections.Generic;

namespace CustomRegionEditor.Web.Interfaces
{
    public interface ISessionStore
    {
        void Save(CustomRegionGroupViewModel item);

        CustomRegionGroupViewModel Get();

        void Clear();

        void ClearHighlight();

        void SetHighlight(List<string> ids);

        List<string> GetHighlight();
    }
}
