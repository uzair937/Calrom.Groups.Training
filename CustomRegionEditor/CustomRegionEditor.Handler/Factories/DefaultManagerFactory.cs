using CustomRegionEditor.Database.Factories;
using CustomRegionEditor.Handler.Interfaces;
using NHibernate;

namespace CustomRegionEditor.Handler.Factories
{
    public class DefaultManagerFactory : IManagerFactory
    {
        private readonly IConverterFactory ConverterFactory;
        private readonly IRepositoryFactory RepositoryFactory;
        private readonly IValidatorFactory ValidatorFactory;

        public DefaultManagerFactory(IConverterFactory converterFactory, IRepositoryFactory repositoryFactory, IValidatorFactory validatorFactory)
        {
            this.ConverterFactory = converterFactory;
            this.RepositoryFactory = repositoryFactory;
            this.ValidatorFactory = validatorFactory;
        }

        public ICustomRegionManager CreateCustomRegionManager(ISession session)
        {
            var modelConverter = this.ConverterFactory.CreateModelConverter(session);
            return new CustomRegionManager(modelConverter, this.RepositoryFactory, this.ValidatorFactory, session);
        }

        public ISearchEntry CreateSearchEntryManager(ISession session)
        {
            var repofactory = new DefaultRepositoryFactory();
            var modelConverter = this.ConverterFactory.CreateModelConverter(session);
            var entryRepository = repofactory.CreateCustomRegionEntryRepository(session);
            return new SearchEntry(entryRepository, modelConverter);
        }

        public ISearchRegion CreateSearchRegionManager(ISession session)
        {
            var repofactory = new DefaultRepositoryFactory();
            var modelConverter = this.ConverterFactory.CreateModelConverter(session);
            var groupRepository = repofactory.CreateCustomRegionGroupRepository(session);
            return new SearchRegion(groupRepository, modelConverter);
        }
    }
}
