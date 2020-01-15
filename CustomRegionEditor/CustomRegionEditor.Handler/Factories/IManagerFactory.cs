using CustomRegionEditor.Handler.Interfaces;
using NHibernate;

namespace CustomRegionEditor.Handler.Factories
{
    public interface IManagerFactory
    {
        ICustomRegionManager CreateCustomRegionManager(ISession session);

        ISearchRegion CreateSearchRegionManager(ISession session);

        ISearchEntry CreateSearchEntryManager(ISession session);
    }
}
