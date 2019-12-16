using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database.Repositories
{
    public class RegionRepo : ISubRegionRepo<RegionModel>
    {
        private ISessionManager SessionManager { get; }

        private IEagerLoader LazyLoader { get; }

        private ISubRegionRepo<CountryModel> CountryRepo { get; }

        public RegionRepo(IEagerLoader lazyLoader, ISessionManager sessionManager, ISubRegionRepo<CountryModel> countryRepo)
        {
            this.CountryRepo = countryRepo;
            this.LazyLoader = lazyLoader;
            this.SessionManager = sessionManager;
        }

        public RegionModel FindByName(string entry)
        {
            var regionModel = new RegionModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                regionModel = dbSession.Query<RegionModel>().FirstOrDefault(a => a.RegionName == (entry));
                if (regionModel == null) regionModel = dbSession.Query<RegionModel>().FirstOrDefault(a => a.RegionId == (entry));
                return LazyLoader.LoadEntities(regionModel);
            }
        } //searches for a matching region

        public List<CustomRegionEntryModel> GetSubRegions(RegionModel region)
        {
            var CustomRegionEntries = new List<CustomRegionEntryModel>();
            if (region == null) return CustomRegionEntries;
            using (var dbSession = SessionManager.OpenSession())
            {
                var countries = dbSession.Query<CountryModel>().Where(c => c.Region.RegionId == region.RegionId).ToList();

                if (countries.Count > 0)
                {
                    foreach (var country in countries)
                    {
                        CustomRegionEntries.Add(new CustomRegionEntryModel()
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
