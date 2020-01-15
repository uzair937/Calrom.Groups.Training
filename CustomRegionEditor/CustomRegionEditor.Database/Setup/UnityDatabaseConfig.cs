using CustomRegionEditor.Database.Factories;
using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Database.NHibernate;
using CustomRegionEditor.Database.Repositories;
using Unity;

namespace CustomRegionEditor.Database.Setup
{
    public static class UnityDatabaseConfig
    {
        public static UnityContainer RegisterAll(UnityContainer container)
        {
            container = RegisterDatabaseConfiguration(container);

            container = RegisterRepositories(container);

            return container;
        }

        public static UnityContainer RegisterDatabaseConfiguration(UnityContainer container)
        {
            container.RegisterType<ISessionManager, NHibernateSessionManager>();

            container.RegisterSingleton<ISessionFactoryManager, NHibernateSessionFactoryManager>();

            return container;
        }

        public static UnityContainer RegisterRepositories(UnityContainer container)
        {
            container.RegisterType<IRepositoryFactory, DefaultRepositoryFactory>();

            return container;
        }
    }
}