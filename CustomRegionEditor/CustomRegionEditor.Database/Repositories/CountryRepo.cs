using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Repositories
{
    public class CountryRepo : ISubRegionRepo<Country>
    {
        private ISessionManager SessionManager { get; }
        private IEagerLoader LazyLoader { get; }
        private ISubRegionRepo<City> CityRepo { get; }
        private ISubRegionRepo<State> StateRepo { get; }

        public CountryRepo(IEagerLoader lazyLoader, ISessionManager sessionManager, ISubRegionRepo<City> cityRepo, ISubRegionRepo<State> stateRepo)
        {
            this.CityRepo = cityRepo;
            this.StateRepo = stateRepo;
            this.LazyLoader = lazyLoader;
            this.SessionManager = sessionManager;
        }

        public Country FindByName(string entry)
        {
            var countryModel = new Country();
            using (var dbSession = SessionManager.OpenSession())
            {
                countryModel = dbSession.Query<Country>().FirstOrDefault(a => a.Name == (entry));
                if (countryModel == null)
                {
                    countryModel = dbSession.Query<Country>().FirstOrDefault(a => a.Id == (entry));
                }
                
                return LazyLoader.LoadEntities(countryModel);
            }

        } //searches for a matching country


        public List<CustomRegionEntry> GetSubRegions(Country country)
        {
            var CustomRegionEntries = new List<CustomRegionEntry>();
            if (country == null) return CustomRegionEntries;
            using (var dbSession = SessionManager.OpenSession())
            {
                var cities = dbSession.Query<City>().Where(c => c.Country.Id == country.Id).ToList();

                if (cities.Count > 0)
                {
                    foreach (var city in cities)
                    {
                        CustomRegionEntries.Add(new CustomRegionEntry()
                        {
                            City = city
                        });
                        CustomRegionEntries = CustomRegionEntries.Concat(CityRepo.GetSubRegions(city)).ToList();
                    }
                }

                var states = dbSession.Query<State>().Where(s => s.Country.Id == country.Id).ToList();

                if (states.Count > 0)
                {
                    foreach (var state in states)
                    {
                        CustomRegionEntries.Add(new CustomRegionEntry()
                        {
                            State = state
                        });
                        CustomRegionEntries = CustomRegionEntries.Concat(StateRepo.GetSubRegions(state)).ToList();
                    }
                }
            }
            return CustomRegionEntries;
        }
    }
}
