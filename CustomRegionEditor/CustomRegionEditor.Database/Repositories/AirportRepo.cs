using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace CustomRegionEditor.Database.Repositories
{
    internal class AirportRepo : ISubRegionRepo<Airport>
    {
        private ISession Session { get; }

        internal AirportRepo(ISession session)
        {
            this.Session = session;
        }

        public Airport FindByName(string entry)
        {
            var airportModel = new Airport();
            airportModel = Session.Query<Airport>().FirstOrDefault(a => a.Name == (entry));
            if (airportModel == null)
            {
                airportModel = Session.Query<Airport>().FirstOrDefault(a => a.Id == (entry));
            }

            return airportModel;
        }

        public List<CustomRegionEntry> GetSubRegions(Airport model)
        {
            throw new NotImplementedException();
        }

        public List<Airport> List()
        {
            return Session.Query<Airport>().ToList();
        }
    }
}
