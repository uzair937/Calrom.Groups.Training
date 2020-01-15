using System;
using System.Collections.Generic;
using System.Linq;
using CustomRegionEditor.Database.Factories;
using NHibernate;
using CustomRegionEditor.Handler.Interfaces;
using CustomRegionEditor.Handler.Validators;
using CustomRegionEditor.Models;
using CustomRegionEditor.Database.Interfaces;

namespace CustomRegionEditor.Handler
{
    public class CustomRegionManager : ICustomRegionManager
    {
        private readonly IModelConverter ModelConverter;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly ISession Session;

        public CustomRegionManager(IModelConverter modelConverter, IRepositoryFactory repositoryFactory, ISession session)
        {
            this.Session = session;
            this.ModelConverter = modelConverter;
            this.repositoryFactory = repositoryFactory;
        }

        public CustomRegionGroupModel Add(CustomRegionGroupModel customRegionGroupModel)
        {
            if (!new CustomRegionValidator().IsValid(customRegionGroupModel))
            {
                return null;
            }

            var customRegionRepo = repositoryFactory.CreateCustomRegionGroupRepository(this.Session);

            //var removedChildren = this.RemoveSubregions(customRegionGroupModel);

            DeleteRemovedEntries(customRegionGroupModel, customRegionRepo);

            var dbModel = this.ModelConverter.GetDbModel(customRegionGroupModel);
            var addReturn = customRegionRepo.AddOrUpdate(dbModel);
            return this.ModelConverter.GetModel(addReturn);
        }

        private void DeleteRemovedEntries(CustomRegionGroupModel customRegionGroupModel, ICustomRegionGroupRepository customRegionRepo)
        {
            var sessionId = customRegionGroupModel.Id;
            if (!(sessionId == null || sessionId == Guid.Empty))
            {
                var oldModel = customRegionRepo.FindById(sessionId.ToString());
                foreach (var entry in oldModel.CustomRegionEntries)
                {
                    var matchAirport = new CustomRegionEntryModel();
                    var matchCity = new CustomRegionEntryModel();
                    var matchState = new CustomRegionEntryModel();
                    var matchCountry = new CustomRegionEntryModel();
                    var matchRegion = new CustomRegionEntryModel();

                    if (entry.Airport != null) matchAirport = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.Airport?.Name == entry.Airport?.Name);
                    if (entry.City != null) matchCity = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.City?.Name == entry.City?.Name);
                    if (entry.State != null) matchState = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.State?.Name == entry.State?.Name);
                    if (entry.Country != null) matchCountry = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.Country?.Name == entry.Country?.Name);
                    if (entry.Region != null) matchRegion = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.Region?.Name == entry.Region?.Name);

                    if (matchAirport == null || matchCity == null || matchState == null || matchCountry == null || matchRegion == null)
                    {
                        var entryId = entry.Id;
                        if (!(entryId == null || entryId == Guid.Empty))
                        {
                            this.DeleteEntryById(entryId.ToString());
                        }
                    }
                }
            }
        }

        private bool DeleteEntryById(string id)
        {
            var customRegionRepo = repositoryFactory.CreateCustomRegionGroupRepository(this.Session);
            var entryList = customRegionRepo.List();
            var customEntry = entryList.FirstOrDefault(a => a.Id == Guid.Parse(id));
            customRegionRepo.Delete(customEntry);

            return true;
        }

        private int RemoveSubregions(CustomRegionGroupModel customRegionGroupModel)
        {
            var customRegionEntryModels = new List<CustomRegionEntryModel>(customRegionGroupModel.CustomRegionEntries);
            int removedRegions = 0;

            foreach (var customRegionEntryModel in customRegionEntryModels)
            {
                var entryNumber = customRegionGroupModel.CustomRegionEntries.Count;
                if (customRegionGroupModel.CustomRegionEntries != null)
                {
                    var type = customRegionEntryModel.GetLocationType();
                    switch (type)
                    {
                        case "airport":
                            break;
                        case "city":
                            RemoveSubregions(customRegionGroupModel, customRegionEntryModel.City);
                            break;
                        case "state":
                            RemoveSubregions(customRegionGroupModel, customRegionEntryModel.State);
                            break;
                        case "country":
                            RemoveSubregions(customRegionGroupModel, customRegionEntryModel.Country);
                            break;
                        case "region":
                            RemoveSubregions(customRegionGroupModel, customRegionEntryModel.Region);
                            break;
                    }
                }

                var finalNumber = customRegionGroupModel.CustomRegionEntries.Count;
                removedRegions += entryNumber - finalNumber;
            }

            return removedRegions;
        }

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, CityModel cityModel)
        {
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Name != cityModel.Name).ToList();
        }

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, StateModel stateModel)
        {
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Name != stateModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.State?.Name != stateModel.Name).ToList();
        }

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, CountryModel countryModel)
        {
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Country?.Name != countryModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Country?.Name != countryModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.Country?.Name != countryModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.State?.Country?.Name != countryModel.Name).ToList();
        }

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, RegionModel regionModel)
        {
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Country?.Region?.Name != regionModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Country?.Region?.Name != regionModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.Country?.Region?.Name != regionModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.State?.Country?.Region?.Name != regionModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Country?.Region?.Name != regionModel.Name).ToList();
        }
        public RegionListModel GetRegionList()
        {
            var regionRepo = repositoryFactory.CreateRegionRepository(this.Session);
            var countryRepo = repositoryFactory.CreateCountryRepository(this.Session);
            var stateRepo = repositoryFactory.CreateStateRepository(this.Session);
            var cityRepo = repositoryFactory.CreateCityRepository(this.Session);
            var airportRepo = repositoryFactory.CreateAirportRepository(this.Session);
            var regionLists = new RegionListModel
            {
                Regions = this.ModelConverter.GetModel(regionRepo.List()),
                Countries = this.ModelConverter.GetModel(countryRepo.List()),
                States = this.ModelConverter.GetModel(stateRepo.List()),
                Cities = this.ModelConverter.GetModel(cityRepo.List()),
                Airports = this.ModelConverter.GetModel(airportRepo.List()),
            };
            return regionLists;
        }

        public CustomRegionGroupModel GetSubRegions(string searchTerm, string filter)
        {
            var regionRepo = repositoryFactory.CreateRegionRepository(this.Session);
            var countryRepo = repositoryFactory.CreateCountryRepository(this.Session);
            var stateRepo = repositoryFactory.CreateStateRepository(this.Session);
            var cityRepo = repositoryFactory.CreateCityRepository(this.Session);
            var customRegionGroupModel = new CustomRegionGroupModel()
            {
                CustomRegionEntries = new List<CustomRegionEntryModel>()
            };
            switch (filter)
            {
                case "regionFilter":
                    var region = regionRepo.FindByName(searchTerm);
                    customRegionGroupModel.CustomRegionEntries = this.ModelConverter.GetModel(regionRepo.GetSubRegions(region));
                    break;

                case "countryFilter":
                    var country = countryRepo.FindByName(searchTerm);
                    customRegionGroupModel.CustomRegionEntries = this.ModelConverter.GetModel(countryRepo.GetSubRegions(country));
                    break;

                case "stateFilter":
                    var state = stateRepo.FindByName(searchTerm);
                    customRegionGroupModel.CustomRegionEntries = this.ModelConverter.GetModel(stateRepo.GetSubRegions(state));
                    break;

                case "cityFilter":
                    var city = cityRepo.FindByName(searchTerm);
                    customRegionGroupModel.CustomRegionEntries = this.ModelConverter.GetModel(cityRepo.GetSubRegions(city));
                    break;

                default:
                    break;
            }
            return customRegionGroupModel;
        }

        public bool DeleteById(string id)
        {
            var customRegionGroupRepository = repositoryFactory.CreateCustomRegionGroupRepository(this.Session);
            try
            {
                var regionList = customRegionGroupRepository.List();
                var customRegion = regionList.FirstOrDefault(a => a.Id == Guid.Parse(id));
                customRegionGroupRepository.Delete(customRegion);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}