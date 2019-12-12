
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity.Lifetime;

namespace CustomRegionEditor.Web.App_Start.UnityExtensions
{
    public class DisposableTransientLifetimeManager : TransientLifetimeManager, IDisposable
    {
        private List<IDisposable> list = new List<IDisposable>();
        
        public override void SetValue(object newValue, ILifetimeContainer container = null)
        {
            base.SetValue(newValue);

            IDisposable disposable = newValue as IDisposable;
            if (disposable != null)
            {
                list.Add(disposable);
            }
        }

        public void Dispose()
        {
            foreach (var item in list)
            {
                item.Dispose();
            }
        }
    }
}