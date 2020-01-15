using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Database.Repositories;
using NHibernate;

namespace CustomRegionEditor.Database.Factories
{
    public class DefaultRepositoryFactory : IRepositoryFactory
    {
        public DefaultRepositoryFactory()
        {
        }

        public ICustomRegionGroupRepository CreateCustomRegionGroupRepository(ISession session)
        {
            return new CustomRegionGroupRepo(session);
        }
        
        public ICustomRegionEntryRepository CreateCustomRegionEntryRepository(ISession session)
        {
            return new CustomRegionEntryRepo(session);
        }

        public ISubRegionRepo<Airport> CreateAirportRepository(ISession session)
        {
            return new AirportRepo(session);
        }

        public ISubRegionRepo<City> CreateCityRepository(ISession session)
        {
            return new CityRepo(session);
        }

        public ISubRegionRepo<State> CreateStateRepository(ISession session)
        {
            return new StateRepo(session);
        }

        public ISubRegionRepo<Country> CreateCountryRepository(ISession session)
        {
            return new CountryRepo(session);
        }

        public ISubRegionRepo<Region> CreateRegionRepository(ISession session)
        {
            return new RegionRepo(session);
        }
    }
}
