using CustomRegionEditor.Database.Factories;
using CustomRegionEditor.Handler.Validators;
using NHibernate;

namespace CustomRegionEditor.Handler.Factories
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IRepositoryFactory RepositoryFactory;
        private readonly IConverterFactory ConverterFactory;

        public ValidatorFactory(IRepositoryFactory repositoryFactory, IConverterFactory converterFactory)
        {
            this.RepositoryFactory = repositoryFactory;
            this.ConverterFactory = converterFactory;
        }

        public CustomRegionEntrySupersetValidator CreateCustomRegionEntrySupersetValidator()
        {
            return new CustomRegionEntrySupersetValidator();
        }

        public CustomRegionEntryValidator CreateCustomRegionEntryValidator(ISession session)
        {
            var modelConverter = this.ConverterFactory.CreateModelConverter(session);
            return new CustomRegionEntryValidator(session, RepositoryFactory, modelConverter);
        }

        public CustomRegionValidator CreateCustomRegionValidator(ISession session)
        {
            return new CustomRegionValidator(session, this, this.RepositoryFactory);
        }
    }
}
