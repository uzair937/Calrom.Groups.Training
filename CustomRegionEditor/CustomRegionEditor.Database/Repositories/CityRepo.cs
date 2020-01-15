using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
namespace CustomRegionEditor.Database.Repositories
{
    internal class CityRepo : ISubRegionRepo<City>
    {
        private ISession Session { get; }

        internal CityRepo(ISession session)
        {
            this.Session = session;
        }

        public City FindByName(string entry)
        {
            var cityModel = new City();

            cityModel = Session.Query<City>().FirstOrDefault(a => a.Name == (entry));
            if (cityModel == null)
            {
                cityModel = Session.Query<City>().FirstOrDefault(a => a.Id == (entry));
            }
            return cityModel;


        } //searches for a matching city

        public List<CustomRegionEntry> GetSubRegions(City city)
        {
            var CustomRegionEntries = new List<CustomRegionEntry>();
            if (city == null) return CustomRegionEntries;

            var airports = Session.Query<Airport>().Where(c => c.City.Id == city.Id).ToList();

            foreach (var airport in airports)
            {
                CustomRegionEntries.Add(new CustomRegionEntry()
                {
                    Airport = airport
                });
            }
            return CustomRegionEntries;
        }

        public List<City> List()
        {
            return Session.Query<City>().ToList();
        }
    }
}
