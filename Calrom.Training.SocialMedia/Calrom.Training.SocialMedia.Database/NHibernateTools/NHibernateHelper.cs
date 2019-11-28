using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate.Caches.SysCache2;
using NHibernate.Tool.hbm2ddl;
using Calrom.Training.SocialMedia.Database.ORMModels;
using Calrom.Training.SocialMedia.Database.Maps;

namespace Calrom.Training.SocialMedia.Database.NHibernateTools
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
            string dbConnection = @"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog = BorkerDB; Integrated Security = True";

            var nHibConfig = Fluently
                .Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(dbConnection))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserModel>())
                .Cache(c => c.ProviderClass<SysCacheProvider>()
                .UseSecondLevelCache());

            sessionFactory = nHibConfig.BuildSessionFactory();

            return sessionFactory;
        }
    }
}
