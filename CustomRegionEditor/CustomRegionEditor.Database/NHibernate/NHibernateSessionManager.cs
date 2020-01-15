using NHibernate;
using CustomRegionEditor.Database.Interfaces;

namespace CustomRegionEditor.Database.NHibernate
{
    public class NHibernateSessionManager : ISessionManager
    {
        private ISessionFactory sessionFactory = null;

        public NHibernateSessionManager(ISessionFactoryManager sessionFactoryManager)
        {
            if (sessionFactory == null)
            {
                sessionFactory = sessionFactoryManager.GetSessionFactory();
            }
        }

        public ISession OpenSession()
        {
            ISession session = null;

            if (session == null || !session.IsOpen)
            {
                session = sessionFactory.OpenSession();
            }

            return session;
        }
    }
}
