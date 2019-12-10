using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Caches.SysCache2;
using CustomRegionEditor.Database.Interfaces;

namespace CustomRegionEditor.Database.Models
{
    public class NHibernateHelper : ISessionManager
    {

        private ISessionFactory sessionFactory = null;
        public ISessionFactory SessionFactory
        {
            get
            {
                return sessionFactory;
            }
        }
        public NHibernateHelper()
        {
            if (sessionFactory == null)
            {
                sessionFactory = this.InitialiseSession();
            }
        }

        public ISession OpenSession()
        {
            return SessionFactory.OpenSession();
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
