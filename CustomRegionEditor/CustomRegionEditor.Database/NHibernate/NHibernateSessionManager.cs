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

        public NHibernateSessionManager(ISessionFactoryManager nHibernateSessionFactoryManager)
        {
            if (sessionFactory == null)
            {
                sessionFactory = nHibernateSessionFactoryManager.GetSessionFactory();
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
            if (session != null || session.IsOpen)
            {
                session.Dispose();
            }
        }
    }
}
