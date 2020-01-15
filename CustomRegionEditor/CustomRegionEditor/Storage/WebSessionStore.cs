using CustomRegionEditor.ViewModels;
using CustomRegionEditor.Web.Interfaces;
using System.Web.SessionState;

namespace CustomRegionEditor.Web.Storage
{
    public class WebSessionStore : ISessionStore
    {
        private readonly HttpSessionState httpSessionState;

        public WebSessionStore(HttpSessionState httpSessionState)
        {
            this.httpSessionState = httpSessionState;
        }

        public CustomRegionGroupViewModel Get()
        {
            return this.httpSessionState["data"] as CustomRegionGroupViewModel;
        }

        public void Save(CustomRegionGroupViewModel item)
        {
            this.httpSessionState["data"] = item;
        }

        public void Clear()
        {
            this.httpSessionState["data"] = new CustomRegionGroupViewModel();
        }
    }
}
