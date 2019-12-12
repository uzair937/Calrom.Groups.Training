using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Caches.SysCache2;
using CustomRegionEditor.Database.Interfaces;
using System;

namespace CustomRegionEditor.Database.Models
{
    public class NHibernateSessionManager : ISessionManager
    {

        private ISessionFactory sessionFactory = null;

        private ISession session = null;

        public NHibernateSessionManager(ISessionFactoryManager sessionFactoryManager)
        {
            if (sessionFactory == null)
            {
                sessionFactory = sessionFactoryManager.GetSessionFactory();
            }
        }

        public ISession OpenSession()
        {
            if (session == null || !session.IsOpen)
            {
                session = sessionFactory.OpenSession();
            }
            return session;
        }

        public void Dispose()
        {
            if (session != null && session.IsOpen)
            {
                session.Dispose();
            }
        }
    }
}
