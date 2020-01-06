using CustomRegionEditor.Database;
using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Database.NHibernate;
using CustomRegionEditor.Database.Repositories;
using CustomRegionEditor.Handler.Converters;
using CustomRegionEditor.Handler.Interfaces;
using CustomRegionEditor.Models;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace CustomRegionEditor.Handler
{
    public static class UnityDatabaseConfig
    {

        public static UnityContainer RegisterComponents(UnityContainer container)
        {

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IEagerLoader, EagerLoader>();

            container.RegisterType<ICustomRegionGroupRepository, CustomRegionGroupRepo>();

            container.RegisterType<ICustomRegionEntryRepository, CustomRegionEntryRepo>();
            
            container.RegisterType<IModelConverter, ModelConverter>();
            
            container.RegisterType<ISessionManager, NHibernateSessionManager>();

            container.RegisterSingleton<ISessionFactoryManager, NHibernateSessionFactoryManager>();

            container.RegisterSingleton<ISubRegionRepo<Region>, RegionRepo>();

            container.RegisterSingleton<ISubRegionRepo<Country>, CountryRepo>();
            
            container.RegisterSingleton<ISubRegionRepo<State>, StateRepo>();

            container.RegisterSingleton<ISubRegionRepo<City>,  CityRepo>();

            container.RegisterSingleton<ISubRegionRepo<Airport>, AirportRepo>();

            return container;
        }
    }
}