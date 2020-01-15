using CustomRegionEditor.Database.Factories;
using CustomRegionEditor.Handler.Converters;
using CustomRegionEditor.Handler.Interfaces;
using NHibernate;

namespace CustomRegionEditor.Handler.Factories
{
    public class ConverterFactory : IConverterFactory
    {
        private readonly IRepositoryFactory repositoryFactory;

        public ConverterFactory(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public IModelConverter CreateModelConverterManager(ISession session)
        {
            return new ModelConverter(this.repositoryFactory, session);
        }
    }
}
