using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using NHibernate;

namespace CustomRegionEditor.Database.Factories
{
    public interface IRepositoryFactory
    {
        ICustomRegionGroupRepository CreateCustomRegionGroupRepository(ISession session);

        ICustomRegionEntryRepository CreateCustomRegionEntryRepository(ISession session);

        ISubRegionRepo<Airport> CreateAirportRepository(ISession session);

        ISubRegionRepo<City> CreateCityRepository(ISession session);

        ISubRegionRepo<State> CreateStateRepository(ISession session);

        ISubRegionRepo<Country> CreateCountryRepository(ISession session);

        ISubRegionRepo<Region> CreateRegionRepository(ISession session);

    }
}
