using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Conventions.Helpers;
using NHibernate.Caches.SysCache2;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Calrom.Training.AuctionHouse.Database
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
            string dbConnection = @"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog = AuctionDB; Integrated Security = True";
            var nhConfig = Fluently.Configure().Database(MsSqlConfiguration.MsSql2012.ConnectionString(dbConnection)).
                Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserModel>()).
                ExposeConfiguration(cfg => new SchemaExport(cfg).
                Create(true, true)).Cache(c => c.ProviderClass<SysCacheProvider>().UseSecondLevelCache());

            sessionFactory = nhConfig.BuildSessionFactory();
            return sessionFactory;
        }
    }
}
