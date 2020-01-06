using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Repositories
{
    public class AirportRepo : ISubRegionRepo<Airport>
    {
        private ISessionManager SessionManager { get; }

        private IEagerLoader EagerLoader { get; }

        public AirportRepo(IEagerLoader eagerLoader, ISessionManager sessionManager)
        {
            this.EagerLoader = eagerLoader;
            this.SessionManager = sessionManager;
        }

        public Airport FindByName(string entry)
        {
            var airportModel = new Airport();
            using (var dbSession = SessionManager.OpenSession())
            {
                airportModel = dbSession.Query<Airport>().FirstOrDefault(a => a.Name == (entry));
                if (airportModel == null)
                {
                    airportModel = dbSession.Query<Airport>().FirstOrDefault(a => a.Id == (entry));
                }

                return EagerLoader.LoadEntities(airportModel);
            }
        }

        public List<CustomRegionEntry> GetSubRegions(Airport model)
        {
            throw new NotImplementedException();
        }
    }
}
