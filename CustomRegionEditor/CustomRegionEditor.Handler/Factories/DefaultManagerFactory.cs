using CustomRegionEditor.Database.Factories;
using CustomRegionEditor.Handler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Handler.Factories
{
    public class DefaultManagerFactory : IManagerFactory
    {
        private readonly IModelConverter modelConverter;
        private readonly IRepositoryFactory repositoryFactory;

        public DefaultManagerFactory(IModelConverter modelConverter, IRepositoryFactory repositoryFactory)
        {
            this.modelConverter = modelConverter;
            this.repositoryFactory = repositoryFactory;
        }

        public ICustomRegionManager CreateCustomRegionManager()
        {
            return new CustomRegionManager(this.modelConverter, this.repositoryFactory);
        }
    }
}
