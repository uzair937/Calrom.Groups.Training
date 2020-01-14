using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Repositories;

namespace CustomRegionEditor.Database.Factories
{
    public class DefaultRepositoryFactory : IRepositoryFactory
    {
        private readonly IEagerLoader eagerLoader;

        private readonly ISessionManager sessionManager;

        public DefaultRepositoryFactory(IEagerLoader eagerLoader, ISessionManager sessionManager)
        {
            this.eagerLoader = eagerLoader;
            this.sessionManager = sessionManager;
        }

        public ICustomRegionGroupRepository CreateCustomRegionGroupRepository()
        {
            return new CustomRegionGroupRepo(this.eagerLoader, this.sessionManager);
        }
    }
}
