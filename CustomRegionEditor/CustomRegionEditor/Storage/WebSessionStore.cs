using CustomRegionEditor.ViewModels;
using CustomRegionEditor.Web.Interfaces;
using System.Collections.Generic;
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
            var model = this.httpSessionState["data"] as CustomRegionGroupViewModel;
            var cloned = model.Clone() as CustomRegionGroupViewModel;
            return cloned;
        }

        public void Save(CustomRegionGroupViewModel item)
        {
            this.httpSessionState["data"] = item;
        }

        public void Clear()
        {
            this.httpSessionState["data"] = new CustomRegionGroupViewModel();
        }
        
        public void ClearHighlight()
        {
            this.httpSessionState["idList"] = new List<string>();
        }

        public void SetHighlight(List<string> ids)
        {
            this.httpSessionState["idList"] = ids;
        }

        public List<string> GetHighlight()
        {
            var model = this.httpSessionState["idList"] as List<string>;
            return model;
        }
    }
}
