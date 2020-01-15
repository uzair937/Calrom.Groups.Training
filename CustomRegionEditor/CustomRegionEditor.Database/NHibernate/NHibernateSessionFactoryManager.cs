using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Caches.SysCache2;
using NHibernate.Tool.hbm2ddl;

namespace CustomRegionEditor.Database.NHibernate
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
                Mappings(m => m.FluentMappings.AddFromAssemblyOf<CustomRegionEntry>()).
                ExposeConfiguration(cfg => new SchemaExport(cfg).
                Create(true, false)).Cache(c => c.ProviderClass<SysCacheProvider>().UseSecondLevelCache());

            sessionFactory = nhConfig.BuildSessionFactory();
            return sessionFactory;
        }
    }
}
