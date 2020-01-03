using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Caches.SysCache2;
using CustomRegionEditor.Database.Interfaces;
using System;

namespace CustomRegionEditor.Database.Models
{
    public class NHibernateSessionFactoryManager : ISessionFactoryManager
    {

        private ISessionFactory sessionFactory = null;

        public ISessionFactory GetSessionFactory()
        {
            return sessionFactory;
        }

        public NHibernateSessionFactoryManager()
        {
            if (sessionFactory == null)
            {
                sessionFactory = this.InitialiseSession();
            }
        }

        public ISessionFactory InitialiseSession()
        {
            string dbConnection = @"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog = CalromGroupsDev; Integrated Security = True";

            var nhConfig = Fluently.Configure().Database(MsSqlConfiguration.MsSql2012.ConnectionString(dbConnection)).
                Mappings(m => m.FluentMappings.AddFromAssemblyOf<CustomRegionEntryModel>()).
                ExposeConfiguration(cfg => new SchemaExport(cfg).
                Create(true, false)).Cache(c => c.ProviderClass<SysCacheProvider>().UseSecondLevelCache());

            sessionFactory = nhConfig.BuildSessionFactory();
            return sessionFactory;
        }
    }
}
