using CustomRegionEditor.Database.Factories;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.EntityMapper;
using CustomRegionEditor.Handler.Interfaces;
using CustomRegionEditor.Models;
using NHibernate;
using System.Collections.Generic;

namespace CustomRegionEditor.Handler.Converters
{
    public class ModelConverter : IModelConverter
    {
        public ModelConverter(IRepositoryFactory repoFactory, ISession session)
        { 
            this.RepositoryFactory = repoFactory;
            this.Session = session;
        }

        private IRepositoryFactory RepositoryFactory { get; }

        private ISession Session { get; }

        public CustomRegionEntryModel GetModel(CustomRegionEntry customRegionEntry)
        {
            var newModel = AutoMapperConfiguration.GetInstance<CustomRegionEntryModel>(customRegionEntry);
            if (customRegionEntry.Region != null)
            {
                newModel.Region = AutoMapperConfiguration.GetInstance<RegionModel>(customRegionEntry.Region);
                newModel.Country = new CountryModel { Name = string.Empty };
                newModel.State = new StateModel { Name = string.Empty };
                newModel.City = new CityModel { Name = string.Empty };
                newModel.Airport = new AirportModel { Name = string.Empty };
                newModel.Value = customRegionEntry.Region.Id;
                newModel.Name = customRegionEntry.Region.Name;
            }
            else if (customRegionEntry.Country != null)
            {
                newModel.Country = AutoMapperConfiguration.GetInstance<CountryModel>(customRegionEntry.Country);
                newModel.Region = new RegionModel { Name = string.Empty };
                newModel.State = new StateModel { Name = string.Empty };
                newModel.City = new CityModel { Name = string.Empty };
                newModel.Airport = new AirportModel { Name = string.Empty };
                newModel.Value = customRegionEntry.Country.Id;
                newModel.Name = customRegionEntry.Country.Name;
            }
            else if (customRegionEntry.State != null)
            {
                newModel.State = AutoMapperConfiguration.GetInstance<StateModel>(customRegionEntry.State);
                newModel.Country = new CountryModel { Name = string.Empty };
                newModel.Region = new RegionModel { Name = string.Empty };
                newModel.City = new CityModel { Name = string.Empty };
                newModel.Airport = new AirportModel { Name = string.Empty };
                newModel.Value = customRegionEntry.State.Id;
                newModel.Name = customRegionEntry.State.Name;
            }
            else if (customRegionEntry.City != null)
            {
                newModel.City = AutoMapperConfiguration.GetInstance<CityModel>(customRegionEntry.City);
                newModel.Country = new CountryModel { Name = string.Empty };
                newModel.State = new StateModel { Name = string.Empty };
                newModel.Region = new RegionModel { Name = string.Empty };
                newModel.Airport = new AirportModel { Name = string.Empty };
                newModel.Value = customRegionEntry.City.Id;
                newModel.Name = customRegionEntry.City.Name;
            }
            else if (customRegionEntry.Airport != null)
            {
                newModel.Airport = AutoMapperConfiguration.GetInstance<AirportModel>(customRegionEntry.Airport);
                newModel.Country = new CountryModel { Name = string.Empty };
                newModel.State = new StateModel { Name = string.Empty };
                newModel.City = new CityModel { Name = string.Empty };
                newModel.Region = new RegionModel { Name = string.Empty };
                newModel.Value = customRegionEntry.Airport.Id;
                newModel.Name = customRegionEntry.Airport.Name;
            }
            return newModel;
        }

        public CustomRegionGroupModel GetModel(CustomRegionGroup customRegionGroup)
        {
            var newModel = AutoMapperConfiguration.GetInstance<CustomRegionGroupModel>(customRegionGroup);
            newModel.CustomRegionEntries = new List<CustomRegionEntryModel>();
            if (customRegionGroup.CustomRegionEntries != null)
            {
                foreach (var cre in customRegionGroup.CustomRegionEntries)
                {
                    newModel.CustomRegionEntries.Add(GetModel(cre));
                }
            }
            return newModel;
        }
        
        public CustomRegionGroup GetDbModel(CustomRegionGroupModel customRegionGroupModel)
        {
            var newModel = AutoMapperConfiguration.GetInstance<CustomRegionGroup>(customRegionGroupModel);
            newModel.CustomRegionEntries = new List<CustomRegionEntry>();
            if (customRegionGroupModel.CustomRegionEntries != null)
            {
                foreach (var cre in customRegionGroupModel.CustomRegionEntries)
                {
                    var newEntry = GetDbModel(cre);
                    newEntry.CustomRegionGroup = newModel;
                    newModel.CustomRegionEntries.Add(newEntry);
                }
            }
            return newModel;
        }

        public CustomRegionEntry GetDbModel(CustomRegionEntryModel customRegionEntryModel)
        {
            var regionRepo = this.RepositoryFactory.CreateRegionRepository(this.Session);
            var countryRepo = this.RepositoryFactory.CreateCountryRepository(this.Session);
            var stateRepo = this.RepositoryFactory.CreateStateRepository(this.Session);
            var cityRepo = this.RepositoryFactory.CreateCityRepository(this.Session);
            var airportRepo = this.RepositoryFactory.CreateAirportRepository(this.Session);

            var newModel = AutoMapperConfiguration.GetInstance<CustomRegionEntry>(customRegionEntryModel);
            if (customRegionEntryModel.Region?.Id != null)
            {
                newModel.Region = regionRepo.FindByName(customRegionEntryModel.Region.Name);
            }
            else if (customRegionEntryModel.Country?.Id != null)
            {
                newModel.Country = countryRepo.FindByName(customRegionEntryModel.Country.Name);
            }
            else if (customRegionEntryModel.State?.Id != null)
            {
                newModel.State = stateRepo.FindByName(customRegionEntryModel.State.Name);
            }
            else if (customRegionEntryModel.City?.Id != null)
            {
                newModel.City = cityRepo.FindByName(customRegionEntryModel.City.Name);
            }
            else if (customRegionEntryModel.Airport?.Id != null)
            {
                newModel.Airport = airportRepo.FindByName(customRegionEntryModel.Airport.Name);
            }
            return newModel;
        }

        public List<CustomRegionGroupModel> GetModel(List<CustomRegionGroup> customRegionGroup)
        {
            var newList = new List<CustomRegionGroupModel>();
            foreach (var model in customRegionGroup)
            {
                newList.Add(GetModel(model));
            }
            return newList;
        }

        public List<CustomRegionEntryModel> GetModel(List<CustomRegionEntry> customRegionGroupViewModels)
        {
            var newList = new List<CustomRegionEntryModel>();
            foreach (var model in customRegionGroupViewModels)
            {
                newList.Add(GetModel(model));
            }
            return newList;
        }

        public List<RegionModel> GetModel(List<Region> regions)
        {
            var newList = new List<RegionModel>();
            foreach (var model in regions)
            {
                newList.Add(GetModel(model));
            }
            return newList;
        }

        public List<CountryModel> GetModel(List<Country> countries)
        {
            var newList = new List<CountryModel>();
            foreach (var model in countries)
            {
                newList.Add(GetModel(model));
            }
            return newList;
        }

        public List<StateModel> GetModel(List<State> states)
        {
            var newList = new List<StateModel>();
            foreach (var model in states)
            {
                newList.Add(GetModel(model));
            }
            return newList;
        }

        public List<CityModel> GetModel(List<City> cities)
        {
            var newList = new List<CityModel>();
            foreach (var model in cities)
            {
                newList.Add(GetModel(model));
            }
            return newList;
        }

        public List<AirportModel> GetModel(List<Airport> airports)
        {
            var newList = new List<AirportModel>();
            foreach (var model in airports)
            {
                newList.Add(GetModel(model));
            }
            return newList;
        }

        public RegionModel GetModel(Region region)
        {
            var newModel = AutoMapperConfiguration.GetInstance<RegionModel>(region);
            return newModel;
        }
        
        public CountryModel GetModel(Country country)
        {
            var newModel = AutoMapperConfiguration.GetInstance<CountryModel>(country);
            return newModel;
        }

        public StateModel GetModel(State state)
        {
            var newModel = AutoMapperConfiguration.GetInstance<StateModel>(state);
            return newModel;
        }

        public CityModel GetModel(City city)
        {
            var newModel = AutoMapperConfiguration.GetInstance<CityModel>(city);
            return newModel;
        }

        public AirportModel GetModel(Airport airport)
        {
            var newModel = AutoMapperConfiguration.GetInstance<AirportModel>(airport);
            return newModel;
        }

    }
}
