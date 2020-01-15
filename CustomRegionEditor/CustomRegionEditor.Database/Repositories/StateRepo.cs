using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace CustomRegionEditor.Database.Repositories
{
    internal class StateRepo : ISubRegionRepo<State>
    {
        private ISession Session { get; }

        internal StateRepo(ISession session)
        {
            this.Session = session;
        }

        public State FindByName(string entry)
        {
            var stateModel = new State();

            stateModel = Session.Query<State>().FirstOrDefault(a => a.Name == (entry));
            if (stateModel == null) stateModel = Session.Query<State>().FirstOrDefault(a => a.Id == (entry));
            return stateModel;


        } //searches for a matching state

        public List<CustomRegionEntry> GetSubRegions(State state)
        {
            var cityModel = new CityRepo(Session);
            var CustomRegionEntries = new List<CustomRegionEntry>();
            if (state == null) return CustomRegionEntries;

            var cities = Session.Query<City>().Where(c => c.State.Id == state.Id).ToList();

            foreach (var city in cities)
            {
                CustomRegionEntries.Add(new CustomRegionEntry()
                {
                    City = city
                });
                CustomRegionEntries = CustomRegionEntries.Concat(cityModel.GetSubRegions(city)).ToList();
            }

            return CustomRegionEntries;
        }
        public List<State> List()
        {
            return Session.Query<State>().ToList();
        }
    }
}
