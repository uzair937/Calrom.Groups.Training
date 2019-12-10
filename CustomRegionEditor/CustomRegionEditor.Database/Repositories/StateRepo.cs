using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Repositories
{
    public class StateRepo : ISubRegionRepo<StateModel>
    {
        private ISessionManager SessionManager { get; }

        private IEagerLoader LazyLoader { get; }

        private ISubRegionRepo<CityModel> CityRepo { get; }

        public StateRepo(IEagerLoader lazyLoader, ISessionManager sessionManager, ISubRegionRepo<CityModel> cityRepo)
        {
            this.CityRepo = cityRepo;
            this.LazyLoader = lazyLoader;
            this.SessionManager = sessionManager;
        }

        public StateModel FindByName(string entry)
        {
            var stateModel = new StateModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                stateModel = dbSession.Query<StateModel>().FirstOrDefault(a => a.StateName == (entry));
                if (stateModel == null) stateModel = dbSession.Query<StateModel>().FirstOrDefault(a => a.StateId == (entry));
                return LazyLoader.LoadEntities(stateModel);
            }

        } //searches for a matching state

        public List<CustomRegionEntryModel> GetSubRegions(StateModel state)
        {
            var CustomRegionEntries = new List<CustomRegionEntryModel>();

            using (var dbSession = SessionManager.OpenSession())
            {
                var cities = dbSession.Query<CityModel>().Where(c => c.State.StateId == state.StateId).ToList();

                foreach (var city in cities)
                {
                    CustomRegionEntries.Add(new CustomRegionEntryModel()
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
