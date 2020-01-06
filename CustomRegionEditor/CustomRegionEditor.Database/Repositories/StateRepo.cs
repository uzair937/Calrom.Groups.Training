using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Repositories
{
    public class StateRepo : ISubRegionRepo<State>
    {
        private ISessionManager SessionManager { get; }

        private IEagerLoader LazyLoader { get; }

        private ISubRegionRepo<City> CityRepo { get; }

        public StateRepo(IEagerLoader lazyLoader, ISessionManager sessionManager, ISubRegionRepo<City> cityRepo)
        {
            this.CityRepo = cityRepo;
            this.LazyLoader = lazyLoader;
            this.SessionManager = sessionManager;
        }

        public State FindByName(string entry)
        {
            var stateModel = new State();
            using (var dbSession = SessionManager.OpenSession())
            {
                stateModel = dbSession.Query<State>().FirstOrDefault(a => a.Name == (entry));
                if (stateModel == null) stateModel = dbSession.Query<State>().FirstOrDefault(a => a.Id == (entry));
                return LazyLoader.LoadEntities(stateModel);
            }

        } //searches for a matching state

        public List<CustomRegionEntry> GetSubRegions(State state)
        {
            var CustomRegionEntries = new List<CustomRegionEntry>();
            if (state == null) return CustomRegionEntries;
            using (var dbSession = SessionManager.OpenSession())
            {
                var cities = dbSession.Query<City>().Where(c => c.State.Id == state.Id).ToList();

                foreach (var city in cities)
                {
                    CustomRegionEntries.Add(new CustomRegionEntry()
                    {
                        City = city
                    });
                    CustomRegionEntries = CustomRegionEntries.Concat(CityRepo.GetSubRegions(city)).ToList();
                }
            }
            return CustomRegionEntries;
        }
    }
}
