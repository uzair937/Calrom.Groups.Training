using NHibernate;

namespace CustomRegionEditor.Database.Interfaces
{
    public interface ISessionFactoryManager
    {
        ISessionFactory GetSessionFactory();

        ISessionFactory InitialiseSession();
    }
}
