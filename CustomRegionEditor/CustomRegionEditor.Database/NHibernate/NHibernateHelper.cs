using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Caches.SysCache2;

namespace CustomRegionEditor.Database.Models
{
    public class NHibernateHelper
    {
        private static ISessionFactory sessionFactory = null;
        public static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                {
                    InitialiseSession();
                }
                return sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static ISessionFactory InitialiseSession()
        {
            string dbConnection = @"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog = CalromGroupsDev; Integrated Security = True";
            var nhConfig = Fluently.Configure().Database(MsSqlConfiguration.MsSql2012.ConnectionString(dbConnection)).
                Mappings(m => m.FluentMappings.AddFromAssemblyOf<CustomRegionEntry>()).
                ExposeConfiguration(cfg => new SchemaExport(cfg).
                Create(true, true)).Cache(c => c.ProviderClass<SysCacheProvider>().UseSecondLevelCache());

            sessionFactory = nhConfig.BuildSessionFactory();
            return sessionFactory;
        }
    }
}
