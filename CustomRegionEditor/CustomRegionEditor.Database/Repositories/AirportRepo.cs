using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Repositories
{
    public class AirportRepo : ISubRegionRepo<AirportModel>
    {
        private ISessionManager SessionManager { get; }

        private IEagerLoader EagerLoader { get; }

        public AirportRepo(IEagerLoader eagerLoader, ISessionManager sessionManager)
        {
            this.EagerLoader = eagerLoader;
            this.SessionManager = sessionManager;
        }

        public AirportModel FindByName(string entry)
        {
            var airportModel = new AirportModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                airportModel = dbSession.Query<AirportModel>().FirstOrDefault(a => a.Name == (entry));
                if (airportModel == null)
                {
                    airportModel = dbSession.Query<AirportModel>().FirstOrDefault(a => a.Id == (entry));
                }

                return EagerLoader.LoadEntities(airportModel);
            }
        }

        public List<CustomRegionEntryModel> GetSubRegions(AirportModel model)
        {
            throw new NotImplementedException();
        }
    }
}
