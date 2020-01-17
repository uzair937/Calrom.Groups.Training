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
    }
}