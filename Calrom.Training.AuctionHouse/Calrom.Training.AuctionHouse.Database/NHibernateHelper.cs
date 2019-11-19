using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
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
            return sessionFactory.OpenSession();
        }
        public static ISessionFactory InitialiseSession()
        {
            string dbConnection = "";
            sessionFactory = Fluently.Configure().Database(MsSqlConfiguration.MsSql2012.ConnectionString(dbConnection)).
                Mappings(m => m.FluentMappings.AddFromAssemblyOf<BidDatabaseModel>()).
                ExposeConfiguration(cfg => new SchemaExport(cfg).
                Create(true, true)).BuildSessionFactory();
            return sessionFactory;
        }
    }
}
