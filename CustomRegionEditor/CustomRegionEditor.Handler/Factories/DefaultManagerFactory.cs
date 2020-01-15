using CustomRegionEditor.Database.Factories;
using CustomRegionEditor.Handler.Interfaces;
using NHibernate;

namespace CustomRegionEditor.Handler.Factories
{
    public class DefaultManagerFactory : IManagerFactory
    {
        private readonly IConverterFactory ConverterFactory;
        private readonly IRepositoryFactory repositoryFactory;

        public DefaultManagerFactory(IConverterFactory converterFactory, IRepositoryFactory repositoryFactory)
        {
            this.ConverterFactory = converterFactory;
            this.repositoryFactory = repositoryFactory;
        }

        public ICustomRegionManager CreateCustomRegionManager(ISession session)
        {
            var modelConverter = this.ConverterFactory.CreateModelConverterManager(session);
            return new CustomRegionManager(modelConverter, this.repositoryFactory, session);
        }

        public ISearchEntry CreateSearchEntryManager(ISession session)
        {
            var repofactory = new DefaultRepositoryFactory();
            var modelConverter = this.ConverterFactory.CreateModelConverterManager(session);
            var entryRepository = repofactory.CreateCustomRegionEntryRepository(session);
            return new SearchEntry(entryRepository, modelConverter);
        }

        public ISearchRegion CreateSearchRegionManager(ISession session)
        {
            var repofactory = new DefaultRepositoryFactory();
            var modelConverter = this.ConverterFactory.CreateModelConverterManager(session);
            var groupRepository = repofactory.CreateCustomRegionGroupRepository(session);
            return new SearchRegion(groupRepository, modelConverter);
        }
    }
}
