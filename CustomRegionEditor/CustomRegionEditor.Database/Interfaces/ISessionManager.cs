using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Caches.SysCache2;
using System;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface ISessionManager
    {
        ISession OpenSession();
    }
}
