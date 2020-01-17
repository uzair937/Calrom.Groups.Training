using System;
using System.Collections.Generic;
using System.Linq;
using CustomRegionEditor.Database.Factories;
using NHibernate;
using CustomRegionEditor.Handler.Interfaces;
using CustomRegionEditor.Handler.Validators;
using CustomRegionEditor.Models;
using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Handler.Factories;
using System.Diagnostics;
using log4net;
using CustomRegionEditor.Database.Models;

namespace CustomRegionEditor.Handler
{
    public class CustomRegionManager : ICustomRegionManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CustomRegionManager));
        private readonly IModelConverter ModelConverter;
        private readonly IRepositoryFactory RepositoryFactory;
        private readonly IValidatorFactory ValidatorFactory;
        private readonly ISession Session;

        public CustomRegionManager(IModelConverter modelConverter, IRepositoryFactory repositoryFactory, IValidatorFactory validatorFactory, ISession session)
        {
            this.Session = session;
            this.ModelConverter = modelConverter;
            this.RepositoryFactory = repositoryFactory;
            this.ValidatorFactory = validatorFactory;

        }

        public ManagerResult<CustomRegionGroupModel> Add(CustomRegionGroupModel customRegionGroupModel)
        {
            var validator = this.ValidatorFactory.CreateCustomRegionValidator(this.Session);
            var customRegionRepo = this.RepositoryFactory.CreateCustomRegionGroupRepository(this.Session);

            var validationModel = validator.IsValid(customRegionGroupModel);

            DeleteRemovedEntries(customRegionGroupModel, customRegionRepo);

            var dbModel = this.ModelConverter.GetDbModel(customRegionGroupModel);

            var addReturn = customRegionRepo.AddOrUpdate(dbModel);

            var result = this.ModelConverter.GetModel(addReturn);

            return new ManagerResult<CustomRegionGroupModel>(validationModel, result);
        }

        private void DeleteRemovedEntries(CustomRegionGroupModel customRegionGroupModel, ICustomRegionGroupRepository customRegionRepo)
        {
            var sessionGroupModelId = customRegionGroupModel.Id;
            var idList = new List<Guid>();
            if (!(sessionGroupModelId == null || sessionGroupModelId == Guid.Empty))
            {
                var oldModel = customRegionRepo.FindById(sessionGroupModelId);
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
                            idList.Add(entryId);
                        }
                    }
                }

                var customRegionEntryRepo = this.RepositoryFactory.CreateCustomRegionEntryRepository(this.Session);

                foreach (var id in idList)
                {
                    customRegionEntryRepo.DeleteById(id);
                }
            }
        }

        public RegionListModel GetRegionList()
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            Logger.Debug("Started GetRegionList");

            var regionRepo = RepositoryFactory.CreateRegionRepository(this.Session);
            var countryRepo = RepositoryFactory.CreateCountryRepository(this.Session);
            var stateRepo = RepositoryFactory.CreateStateRepository(this.Session);
            var cityRepo = RepositoryFactory.CreateCityRepository(this.Session);
            var airportRepo = RepositoryFactory.CreateAirportRepository(this.Session);

            var regions = regionRepo.List();
            var countries = countryRepo.List();
            var states = stateRepo.List();
            var cities = cityRepo.List();
            var airports = airportRepo.List();

            var regionLists = new RegionListModel
            {
                Regions = this.ModelConverter.GetModel(regions),
                Countries = this.ModelConverter.GetModel(countries),
                States = this.ModelConverter.GetModel(states),
                Cities = this.ModelConverter.GetModel(cities),
                Airports = this.ModelConverter.GetModel(airports),
            };

            stopwatch.Stop();

            Logger.DebugFormat("Finished GetRegionList. Time elapsed: {0}ms", stopwatch.Elapsed.TotalMilliseconds);

            return regionLists;
        }

        public bool DeleteById(string id)
        {
            var customRegionGroupRepository = RepositoryFactory.CreateCustomRegionGroupRepository(this.Session);
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

        public ManagerResult<CustomRegionGroupModel> RemoveRegions(CustomRegionGroupModel customRegionGroupModel, CustomRegionEntryModel customRegionEntryModel)
        {
            var returnSubResult = RemoveSubRegions(customRegionGroupModel, customRegionEntryModel);
            var returnSuperResult = RemoveSuperRegions(returnSubResult.Object, customRegionEntryModel);

            returnSubResult.ValidationResult.Merge(returnSuperResult.ValidationResult);

            return returnSubResult;
        }

        private ManagerResult<CustomRegionGroupModel> RemoveSuperRegions(CustomRegionGroupModel customRegionGroupModel, CustomRegionEntryModel customRegionEntryModel)
        {
            var regionRepo = this.RepositoryFactory.CreateRegionRepository(Session);
            var countryRepo = this.RepositoryFactory.CreateCountryRepository(Session);
            var stateRepo = this.RepositoryFactory.CreateStateRepository(Session);
            var cityRepo = this.RepositoryFactory.CreateCityRepository(Session);
            var airportRepo = this.RepositoryFactory.CreateAirportRepository(Session);

            var parentEntry = new CustomRegionEntryModel();
            var removeEntry = new CustomRegionEntryModel();
            parentEntry = null;

            var entry = customRegionEntryModel;

            var entryType = entry.GetLocationType();
            var searchId = entry.LocationId;

            if (entryType.Equals("country"))  //COUNTRY
            {
                var foundRegion = new Country();

                foundRegion = countryRepo.Find(searchId);
                var parentRegion = foundRegion.Region;

                var findParent = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationId == parentRegion.Id && a.LocationName == parentRegion.Name);

                if (findParent != null)
                {
                    parentEntry = findParent;
                    removeEntry = entry;
                }
            }
            else if (entryType.Equals("state")) //STATE
            {
                var foundRegion = new State();

                foundRegion = stateRepo.Find(searchId);
                var parentCountry = foundRegion.Country;
                var parentRegion = parentCountry.Region;

                var findParentRegion = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationId == parentRegion.Id && a.LocationName == parentRegion.Name);
                var findParentCountry = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationId == parentCountry.Id && a.LocationName == parentCountry.Name);

                if (findParentRegion != null)
                {
                    parentEntry = findParentRegion;
                    removeEntry = entry;
                }
                if (findParentCountry != null)
                {
                    parentEntry = findParentCountry;
                    removeEntry = entry;
                }

            }
            else if (entryType.Equals("city")) //CITY
            {
                var foundRegion = new City();

                foundRegion = cityRepo.Find(searchId);
                var parentState = foundRegion.State;
                var parentCountry = foundRegion.Country;
                var parentRegion = parentCountry.Region;

                var findParentRegion = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationId == parentRegion.Id && a.LocationName == parentRegion.Name);
                var findParentCountry = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationId == parentCountry.Id && a.LocationName == parentCountry.Name);

                if (parentState != null)
                {
                    var findParentState = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationId == parentState.Id && a.LocationName == parentState.Name);
                    if (findParentState != null)
                    {
                        parentEntry = findParentState;
                        removeEntry = entry;
                    }
                }
                if (findParentRegion != null)
                {
                    parentEntry = findParentRegion;
                    removeEntry = entry;
                }
                if (findParentCountry != null)
                {
                    parentEntry = findParentCountry;
                    removeEntry = entry;
                }
            }
            else if (entryType.Equals("airport")) //AIRPORT
            {
                var foundRegion = new Airport();

                foundRegion = airportRepo.Find(searchId);
                var parentCity = foundRegion.City;
                var parentState = parentCity.State;
                var parentCountry = parentCity.Country;
                var parentRegion = parentCountry.Region;

                var findParentRegion = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationId == parentRegion.Id && a.LocationName == parentRegion.Name);
                var findParentCountry = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationId == parentCountry.Id && a.LocationName == parentCountry.Name);
                var findParentCity = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationId == parentCity.Id && a.LocationName == parentCity.Name);

                if (parentState != null)
                {
                    var findParentState = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationId == parentState.Id && a.LocationName == parentState.Name);
                    if (findParentState != null)
                    {
                        parentEntry = findParentState;
                        removeEntry = entry;
                    }
                }
                if (findParentRegion != null)
                {
                    parentEntry = findParentRegion;
                    removeEntry = entry;
                }
                if (findParentCountry != null)
                {
                    parentEntry = findParentCountry;
                    removeEntry = entry;
                }
                if (findParentCity != null)
                {
                    parentEntry = findParentCity;
                    removeEntry = entry;
                }
            }

            var validationModel = new ValidationModel();
            if (parentEntry != null)
            {
                var errorModel = new ErrorModel();
                errorModel.Message = "This region is a part of " + parentEntry.LocationName;
                errorModel.Warning = true;
                validationModel.Errors.Add(errorModel);
                customRegionGroupModel.CustomRegionEntries.Remove(removeEntry);
            }

            return new ManagerResult<CustomRegionGroupModel>(validationModel, customRegionGroupModel);
        }

        private ManagerResult<CustomRegionGroupModel> RemoveSubRegions(CustomRegionGroupModel customRegionGroupModel, CustomRegionEntryModel customRegionEntryModel)
        {
            var validationModel = new ValidationModel();
            var removedEntries = new List<CustomRegionEntryModel>();
            var duplicate = false;

            var regionRepo = this.RepositoryFactory.CreateRegionRepository(Session);
            var countryRepo = this.RepositoryFactory.CreateCountryRepository(Session);
            var stateRepo = this.RepositoryFactory.CreateStateRepository(Session);
            var cityRepo = this.RepositoryFactory.CreateCityRepository(Session);
            var airportRepo = this.RepositoryFactory.CreateAirportRepository(Session);

            var entry = customRegionEntryModel;

            var entryType = entry.GetLocationType();
            var searchId = entry.LocationId;

            if (entryType.Equals("region")) //REGION
            {
                var foundRegion = new Region();

                foundRegion = regionRepo.Find(searchId);
                var duplicateRegion = customRegionGroupModel.CustomRegionEntries.Where(a => a.LocationName == foundRegion.Name && a.LocationId == foundRegion.Id).ToList();
                if (duplicateRegion.Count > 1) duplicate = true;

                var subCountries = foundRegion.Countries;

                var subCities = new List<City>();
                var subStates = new List<State>();
                subCountries.ToList().ForEach(a => subCities.AddRange(a.Cities));
                subCountries.ToList().ForEach(a => subStates.AddRange(a.States));

                var subAirports = new List<Airport>();
                subCities.ToList().ForEach(a => subAirports.AddRange(a.Airports));

                foreach (var location in subCountries)
                {
                    var matchRegion = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationName == location.Name && a.LocationId == location.Id);
                    if (matchRegion != null) removedEntries.Add(matchRegion);
                }

                foreach (var location in subCities)
                {
                    var matchRegion = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationName == location.Name && a.LocationId == location.Id);
                    if (matchRegion != null) removedEntries.Add(matchRegion);
                }

                foreach (var location in subStates)
                {
                    var matchRegion = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationName == location.Name && a.LocationId == location.Id);
                    if (matchRegion != null) removedEntries.Add(matchRegion);
                }

                foreach (var location in subAirports)
                {
                    var matchRegion = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationName == location.Name && a.LocationId == location.Id);
                    if (matchRegion != null) removedEntries.Add(matchRegion);
                }
            }
            else if (entryType.Equals("country"))  //COUNTRY
            {
                var foundRegion = new Country();

                foundRegion = countryRepo.Find(searchId);
                var duplicateRegion = customRegionGroupModel.CustomRegionEntries.Where(a => a.LocationName == foundRegion.Name && a.LocationId == foundRegion.Id).ToList();
                if (duplicateRegion.Count > 1) duplicate = true;

                var subCities = foundRegion.Cities;
                var subStates = foundRegion.States;

                var subAirports = new List<Airport>();
                subCities.ToList().ForEach(a => subAirports.AddRange(a.Airports));

                foreach (var location in subCities)
                {
                    var matchRegion = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationName == location.Name && a.LocationId == location.Id);
                    if (matchRegion != null) removedEntries.Add(matchRegion);
                }

                foreach (var location in subStates)
                {
                    var matchRegion = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationName == location.Name && a.LocationId == location.Id);
                    if (matchRegion != null) removedEntries.Add(matchRegion);
                }

                foreach (var location in subAirports)
                {
                    var matchRegion = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationName == location.Name && a.LocationId == location.Id);
                    if (matchRegion != null) removedEntries.Add(matchRegion);
                }
            }
            else if (entryType.Equals("state")) //STATE
            {
                var foundRegion = new State();

                foundRegion = stateRepo.Find(searchId);
                var duplicateRegion = customRegionGroupModel.CustomRegionEntries.Where(a => a.LocationName == foundRegion.Name && a.LocationId == foundRegion.Id).ToList();
                if (duplicateRegion.Count > 1) duplicate = true;

                var subCities = foundRegion.Cities;

                var subAirports = new List<Airport>();
                subCities.ToList().ForEach(a => subAirports.AddRange(a.Airports));

                foreach (var location in subCities)
                {
                    var matchRegion = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationName == location.Name && a.LocationId == location.Id);
                    if (matchRegion != null) removedEntries.Add(matchRegion);
                }

                foreach (var location in subAirports)
                {
                    var matchRegion = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationName == location.Name && a.LocationId == location.Id);
                    if (matchRegion != null) removedEntries.Add(matchRegion);
                }
            }
            else if (entryType.Equals("city")) //CITY
            {
                var foundRegion = new City();

                foundRegion = cityRepo.Find(searchId);
                var duplicateRegion = customRegionGroupModel.CustomRegionEntries.Where(a => a.LocationName == foundRegion.Name && a.LocationId == foundRegion.Id).ToList();
                if (duplicateRegion.Count > 1) duplicate = true;

                var subAirports = foundRegion.Airports;

                foreach (var location in subAirports)
                {
                    var matchRegion = customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a.LocationName == location.Name && a.LocationId == location.Id);
                    if (matchRegion != null) removedEntries.Add(matchRegion);
                }
            }
            else if (entryType.Equals("airport")) //AIRPORT
            {
                var foundRegion = new Airport();

                foundRegion = airportRepo.Find(searchId);
                var duplicateRegion = customRegionGroupModel.CustomRegionEntries.Where(a => a.LocationName == foundRegion.Name && a.LocationId == foundRegion.Id).ToList();
                if (duplicateRegion.Count > 1) duplicate = true;
            }

            if (duplicate)
            {
                var errorModel = new ErrorModel();
                errorModel.Message = "This region has already been added";
                errorModel.Warning = true;
                validationModel.Errors.Add(errorModel);
            }


            if (removedEntries.Count == 1)
            {
                var errorModel = new ErrorModel();
                errorModel.Message = removedEntries.Count + " contained subregion has been removed";
                errorModel.Warning = true;
                validationModel.Errors.Add(errorModel);
            }
            else if (removedEntries.Count > 1)
            {
                var errorModel = new ErrorModel();
                errorModel.Message = removedEntries.Count + " contained subregions have been removed";
                errorModel.Warning = true;
                validationModel.Errors.Add(errorModel);
            }

            foreach (var subregions in removedEntries)
            {
                customRegionGroupModel.CustomRegionEntries.Remove(subregions);
            }
            return new ManagerResult<CustomRegionGroupModel>(validationModel, customRegionGroupModel);
        }
    }
}