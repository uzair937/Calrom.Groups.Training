using CustomRegionEditor.Database;
using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Database.NHibLazyLoader;
using CustomRegionEditor.Database.Repositories;
using CustomRegionEditor.Web.Converters;
using CustomRegionEditor.Web.Interfaces;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace CustomRegionEditor.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IEagerLoader, EagerLoader>();

            container.RegisterType<ICustomRegionGroupRepository, CustomRegionGroupRepo>();

            container.RegisterType<ICustomRegionEntryRepository, CustomRegionEntryRepo>();
            
            container.RegisterType<IViewModelConverter, ViewModelConverter>();
            
            container.RegisterSingleton<ISessionManager, NHibernateHelper>();

            container.RegisterSingleton<ISubRegionRepo<RegionModel>, RegionRepo>();

            container.RegisterSingleton<ISubRegionRepo<CountryModel>, CountryRepo>();
            
            container.RegisterSingleton<ISubRegionRepo<StateModel>, StateRepo>();

            container.RegisterSingleton<ISubRegionRepo<CityModel>,  CityRepo>();

            container.RegisterSingleton<ISubRegionRepo<AirportModel>, AirportRepo>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}