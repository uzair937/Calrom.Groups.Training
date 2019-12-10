using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Repositories
{
    public class CountryRepo : ISubRegionRepo<CountryModel>
    {
        private ISessionManager SessionManager { get; }
        private IEagerLoader LazyLoader { get; }
        private ISubRegionRepo<CityModel> CityRepo { get; }
        private ISubRegionRepo<StateModel> StateRepo { get; }

        public CountryRepo(IEagerLoader lazyLoader, ISessionManager sessionManager, ISubRegionRepo<CityModel> cityRepo, ISubRegionRepo<StateModel> stateRepo)
        {
            this.CityRepo = cityRepo;
            this.StateRepo = stateRepo;
            this.LazyLoader = lazyLoader;
            this.SessionManager = sessionManager;
        }

        public CountryModel FindByName(string entry)
        {
            var countryModel = new CountryModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                countryModel = dbSession.Query<CountryModel>().FirstOrDefault(a => a.CountryName == (entry));
                if (countryModel == null)
                {
                    countryModel = dbSession.Query<CountryModel>().FirstOrDefault(a => a.CountryId == (entry));
                }
                
                return LazyLoader.LoadEntities(countryModel);
            }

        } //searches for a matching country


        public List<CustomRegionEntryModel> GetSubRegions(CountryModel country)
        {
            var CustomRegionEntries = new List<CustomRegionEntryModel>();

            using (var dbSession = SessionManager.OpenSession())
            {
                var cities = dbSession.Query<CityModel>().Where(c => c.Country.CountryId == country.CountryId).ToList();

                if (cities.Count > 0)
                {
                    foreach (var city in cities)
                    {
                        CustomRegionEntries.Add(new CustomRegionEntryModel()
                        {
                            City = city
                        });
                        CustomRegionEntries = CustomRegionEntries.Concat(CityRepo.GetSubRegions(city)).ToList();
                    }
                }

                var states = dbSession.Query<StateModel>().Where(s => s.Country.CountryId == country.CountryId).ToList();

                if (states.Count > 0)
                {
                    foreach (var state in states)
                    {
                        CustomRegionEntries.Add(new CustomRegionEntryModel()
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
