using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Repositories
{
    public class RegionRepo : ISubRegionRepo<Region>
    {
        private ISessionManager SessionManager { get; }

        private IEagerLoader LazyLoader { get; }

        private ISubRegionRepo<Country> CountryRepo { get; }

        public RegionRepo(IEagerLoader eagerLoader, ISessionManager sessionManager, ISubRegionRepo<Country> countryRepo)
        {
            this.CountryRepo = countryRepo;
            this.LazyLoader = eagerLoader;
            this.SessionManager = sessionManager;
        }

        public Region FindByName(string entry)
        {
            var regionModel = new Region();
            using (var dbSession = SessionManager.OpenSession())
            {
                regionModel = dbSession.Query<Region>().FirstOrDefault(a => a.Name == (entry));
                if (regionModel == null) regionModel = dbSession.Query<Region>().FirstOrDefault(a => a.Id == (entry));
                return LazyLoader.LoadEntities(regionModel);
            }
        } //searches for a matching region

        public List<CustomRegionEntry> GetSubRegions(Region region)
        {
            var CustomRegionEntries = new List<CustomRegionEntry>();
            if (region == null) return CustomRegionEntries;
            using (var dbSession = SessionManager.OpenSession())
            {
                var countries = dbSession.Query<Country>().Where(c => c.Region.Id == region.Id).ToList();

                if (countries.Count > 0)
                {
                    foreach (var country in countries)
                    {
                        CustomRegionEntries.Add(new CustomRegionEntry()
                        {
                            Country = country
                        });
                        CustomRegionEntries = CustomRegionEntries.Concat(CountryRepo.GetSubRegions(country)).ToList();
                    }
                }
            }
            return CustomRegionEntries;
        }
    }
}
