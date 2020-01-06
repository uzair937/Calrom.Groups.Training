using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Repositories
{
    public class CityRepo : ISubRegionRepo<City>
    {
        private ISessionManager SessionManager { get; }

        private IEagerLoader LazyLoader { get; }

        public CityRepo(IEagerLoader lazyLoader, ISessionManager sessionManager)
        {
            this.LazyLoader = lazyLoader;
            this.SessionManager = sessionManager;
        }

        public City FindByName(string entry)
        {
            var cityModel = new City();
            using (var dbSession = SessionManager.OpenSession())
            {
                cityModel = dbSession.Query<City>().FirstOrDefault(a => a.Name == (entry));
                if (cityModel == null)
                {
                    cityModel = dbSession.Query<City>().FirstOrDefault(a => a.Id == (entry));
                }
                return LazyLoader.LoadEntities(cityModel);
            }

        } //searches for a matching city

        public List<CustomRegionEntry> GetSubRegions(City city)
        {
            var CustomRegionEntries = new List<CustomRegionEntry>();
            if (city == null) return CustomRegionEntries;
            using (var dbSession = SessionManager.OpenSession())
            {
                var airports = dbSession.Query<Airport>().Where(c => c.City.Id == city.Id).ToList();

                foreach (var airport in airports)
                {
                    CustomRegionEntries.Add(new CustomRegionEntry()
                    {
                        Airport = airport
                    });
                }
            }
            return CustomRegionEntries;
        }
    }
}
