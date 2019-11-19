using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Calrom.Training.SocialMedia.Database.Models;
using NHibernate.Tool.hbm2ddl;


namespace Calrom.Training.SocialMedia.Database.NHibernate
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

        public static ISession openSession()
        {
            return sessionFactory.OpenSession();
        }

        public static ISessionFactory InitialiseSession()
        {
            string dbConnection = @"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog = TestDB; Integrated Security = True";

            sessionFactory = Fluently.Configure().Database(MsSqlConfiguration.MsSql2012.ConnectionString(dbConnection)).
                Mappings(m => m.FluentMappings.AddFromAssemblyOf<BorkModel>()).
                ExposeConfiguration(cfg => new SchemaExport(cfg).
                Create(true, true)).BuildSessionFactory();

            return sessionFactory;
        }
    }
}
