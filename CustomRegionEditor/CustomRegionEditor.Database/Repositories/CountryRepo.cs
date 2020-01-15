using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace CustomRegionEditor.Database.Repositories
{
    internal class CountryRepo : ISubRegionRepo<Country>
    {
        private ISession Session { get; }

        internal CountryRepo(ISession session)
        {
            this.Session = session;
        }

        public Country FindByName(string entry)
        {
            var countryModel = new Country();

            countryModel = Session.Query<Country>().FirstOrDefault(a => a.Name == (entry));
            if (countryModel == null)
            {
                countryModel = Session.Query<Country>().FirstOrDefault(a => a.Id == (entry));
            }

            return countryModel;
        } //searches for a matching country


        public List<CustomRegionEntry> GetSubRegions(Country country)
        {
            var cityRepo = new CityRepo(Session);
            var stateRepo = new StateRepo(Session);
            var CustomRegionEntries = new List<CustomRegionEntry>();
            if (country == null) return CustomRegionEntries;

            var cities = Session.Query<City>()
                .Where(c => c.Country.Id == country.Id)
                .ToList();

            if (cities.Count > 0)
            {
                foreach (var city in cities)
                {
                    CustomRegionEntries.Add(new CustomRegionEntry()
                    {
                        City = city
                    });
                    CustomRegionEntries = CustomRegionEntries.Concat(cityRepo.GetSubRegions(city)).ToList();
                }
            }

            var states = Session.Query<State>().Where(s => s.Country.Id == country.Id).ToList();

            if (states.Count > 0)
            {
                foreach (var state in states)
                {
                    CustomRegionEntries.Add(new CustomRegionEntry()
                    {
                        State = state
                    });
                    CustomRegionEntries = CustomRegionEntries.Concat(stateRepo.GetSubRegions(state)).ToList();
                }
            }

            return CustomRegionEntries;
        }

        public List<Country> List()
        {
            return Session.Query<Country>().ToList();
        }
    }
}
