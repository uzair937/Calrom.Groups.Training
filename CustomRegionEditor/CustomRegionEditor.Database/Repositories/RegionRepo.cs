using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using log4net;
using NHibernate;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CustomRegionEditor.Database.Repositories
{
    internal class RegionRepo : ISubRegionRepo<Region>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RegionRepo));

        private ISession Session { get; }

        internal RegionRepo(ISession session)
        {
            this.Session = session;
        }

        public Region Find(string entry)
        {
            var regionModel = new Region();

            regionModel = Session.Query<Region>().FirstOrDefault(a => a.Id == (entry));
            if (regionModel == null) regionModel = Session.Query<Region>().FirstOrDefault(a => a.Name == (entry));
            return regionModel;

        } //searches for a matching region

        public List<CustomRegionEntry> GetSubRegions(Region region)
        {
            var customRegionEntries = new List<CustomRegionEntry>();
            var countryRepo = new CountryRepo(Session);
            if (region == null) return customRegionEntries;

            var countries = Session.Query<Country>().Where(c => c.Region.Id == region.Id).ToList();

            if (countries.Count > 0)
            {
                foreach (var country in countries)
                {
                    customRegionEntries.Add(new CustomRegionEntry()
                    {
                        Country = country
                    });
                    customRegionEntries = customRegionEntries.Concat(countryRepo.GetSubRegions(country)).ToList();
                }
            }
            return customRegionEntries;
        }

        public List<Region> List()
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            Logger.Debug("Started List");
            
            var query = Session.Query<Region>().ToList();

            stopwatch.Stop();

            Logger.DebugFormat("Finished List. Time elapsed: {0}ms", stopwatch.Elapsed.TotalMilliseconds);

            return query;
        }
    }
}
