using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Repositories
{
    public class CityRepo : ISubRegionRepo<CityModel>
    {
        private ISessionManager SessionManager { get; }

        private IEagerLoader LazyLoader { get; }

        public CityRepo(IEagerLoader lazyLoader, ISessionManager sessionManager)
        {
            this.LazyLoader = lazyLoader;
            this.SessionManager = sessionManager;
        }

        public CityModel FindByName(string entry)
        {
            var cityModel = new CityModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                cityModel = dbSession.Query<CityModel>().FirstOrDefault(a => a.CityName == (entry));
                if (cityModel == null) cityModel = dbSession.Query<CityModel>().FirstOrDefault(a => a.CityId == (entry));
                return LazyLoader.LoadEntities(cityModel);
            }

        } //searches for a matching city

        public List<CustomRegionEntryModel> GetSubRegions(CityModel city)
        {
            var CustomRegionEntries = new List<CustomRegionEntryModel>();

            using (var dbSession = SessionManager.OpenSession())
            {
                var airports = dbSession.Query<AirportModel>().Where(c => c.City.CityId == city.CityId).ToList();

                foreach (var airport in airports)
                {
                    CustomRegionEntries.Add(new CustomRegionEntryModel()
                    {
                        Airport = airport
                    });
                }
            }
            return CustomRegionEntries;
        }
    }
}
