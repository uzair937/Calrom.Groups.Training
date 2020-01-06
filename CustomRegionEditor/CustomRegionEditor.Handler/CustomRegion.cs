using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Handler.Interfaces;
using CustomRegionEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Handler
{
    public class CustomRegion : IRegionHandler
    {
        public CustomRegion(IModelConverter modelConverter, ICustomRegionGroupRepository customRegionGroupRepository, ICustomRegionEntryRepository customRegionEntryRepository, ISubRegionRepo<City> cityRepo, ISubRegionRepo<State> stateRepo, ISubRegionRepo<Country> countryRepo, ISubRegionRepo<Region> regionRepo)
        {
            this.StateRepo = stateRepo;
            this.CityRepo = cityRepo;
            this.CountryRepo = countryRepo;
            this.RegionRepo = regionRepo;
            this.ModelConverter = modelConverter;
            this.CustomRegionGroupRepository = customRegionGroupRepository;
            this.CustomRegionEntryRepository = customRegionEntryRepository;
        }

        private ISubRegionRepo<City> CityRepo { get; }
        private ISubRegionRepo<State> StateRepo { get; }
        private ISubRegionRepo<Country> CountryRepo { get; }
        private ISubRegionRepo<Region> RegionRepo { get; }
        private IModelConverter ModelConverter { get; }

        public ICustomRegionGroupRepository CustomRegionGroupRepository { get; private set; }
        public ICustomRegionEntryRepository CustomRegionEntryRepository { get; private set; }

        public List<List<string>> GetRegionList()
        {
            var regionLists = new List<List<string>>
            {
                this.CustomRegionGroupRepository.GetNames("airport").Distinct().ToList(),
                this.CustomRegionGroupRepository.GetNames("city").Distinct().ToList(),
                this.CustomRegionGroupRepository.GetNames("state").Distinct().ToList(),
                this.CustomRegionGroupRepository.GetNames("country").Distinct().ToList(),
                this.CustomRegionGroupRepository.GetNames("region").Distinct().ToList()
            };
            return regionLists;
        }

        public CustomRegionGroupModel GetSubRegions(string searchTerm, string filter)
        {
            var customRegionGroupModel = new CustomRegionGroupModel()
            {
                CustomRegionEntries = new List<CustomRegionEntryModel>()
            };
            switch (filter)
            {
                case "regionFilter":
                    var region = RegionRepo.FindByName(searchTerm);
                    customRegionGroupModel.CustomRegionEntries = this.ModelConverter.GetModel(RegionRepo.GetSubRegions(region));
                    break;

                case "countryFilter":
                    var country = CountryRepo.FindByName(searchTerm);
                    customRegionGroupModel.CustomRegionEntries = this.ModelConverter.GetModel(CountryRepo.GetSubRegions(country));
                    break;

                case "stateFilter":
                    var state = StateRepo.FindByName(searchTerm);
                    customRegionGroupModel.CustomRegionEntries = this.ModelConverter.GetModel(StateRepo.GetSubRegions(state));
                    break;

                case "cityFilter":
                    var city = CityRepo.FindByName(searchTerm);
                    customRegionGroupModel.CustomRegionEntries = this.ModelConverter.GetModel(CityRepo.GetSubRegions(city));
                    break;

                default:
                    break;
            }
            return customRegionGroupModel;
        }

        public bool DeleteById(string id)
        {
            try
            {
                var regionList = this.CustomRegionGroupRepository.List();
                var customRegion = regionList.FirstOrDefault(a => a.Id == Guid.Parse(id));
                this.CustomRegionGroupRepository.Delete(customRegion);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
