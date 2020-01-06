using CustomRegionEditor.Controllers;
using CustomRegionEditor.Handler;
using CustomRegionEditor.Handler.Converters;
using CustomRegionEditor.Handler.Interfaces;
using CustomRegionEditor.Web.Converters;
using CustomRegionEditor.Web.Interfaces;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Mvc5;

namespace CustomRegionEditor.Web
{
    public static class UnityConfig
    {

        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            
            container.RegisterType<IViewModelConverter, ViewModelConverter>();

            container.RegisterType<IModelConverter, ModelConverter>();

            container.RegisterType<IEntryHandler, CustomEntry>();

            container.RegisterType<IRegionHandler, CustomRegion>();

            container.RegisterType<ISearchEntry, SearchEntry>();

            container.RegisterType<ISearchRegion, SearchRegion>();

            container.RegisterSingleton<ISessionRegionGroupRepository, SessionRegionGroupRepo>();

            container = Handler.UnityDatabaseConfig.RegisterComponents(container);

            //container.RegisterType<HomeController>(new InjectionConstructor());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}