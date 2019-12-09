using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Caches.SysCache2;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface INHibernateHelper
    {
        ISessionFactory InitialiseSession();

        ISession OpenSession();
    }
}
