using CustomRegionEditor.Controllers;
using CustomRegionEditor.Handler;
using CustomRegionEditor.Handler.Converters;
using CustomRegionEditor.Handler.Factories;
using CustomRegionEditor.Handler.Interfaces;
using CustomRegionEditor.Web.Converters;
using CustomRegionEditor.Web.Interfaces;
using CustomRegionEditor.Web.Storage;
using System.Web;
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

            container.RegisterType<IConverterFactory, ConverterFactory>();

            container.RegisterType<IManagerFactory, DefaultManagerFactory>();

            container.RegisterFactory<ISessionStore>(c => new WebSessionStore(HttpContext.Current.Session));

            container = Database.Setup.UnityDatabaseConfig.RegisterAll(container);

            container = Handler.UnityDatabaseConfig.RegisterComponents(container);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}