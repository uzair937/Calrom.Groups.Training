using System;
using System.Collections.Generic;
using System.Linq;
using CustomRegionEditor.Database.Factories;
using CustomRegionEditor.Database.Interfaces;
using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Handler.Interfaces;
using CustomRegionEditor.Handler.Validators;
using CustomRegionEditor.Models;

namespace CustomRegionEditor.Handler
{
    public class CustomRegionManager : ICustomRegionManager
    {
        private readonly IModelConverter modelConverter;
        private readonly IRepositoryFactory repositoryFactory;

        public CustomRegionManager(IModelConverter modelConverter, IRepositoryFactory repositoryFactory)
        {
            this.modelConverter = modelConverter;
            this.repositoryFactory = repositoryFactory;
        }

        public CustomRegionGroupModel Add(CustomRegionGroupModel customRegionGroupModel)
        {
            if (!new CustomRegionValidator().IsValid(customRegionGroupModel))
            {
                return null;
            }

            var customRegionRepo = repositoryFactory.CreateCustomRegionGroupRepository();
            var removedChildren = this.RemoveSubregions(customRegionGroupModel);

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
                            this.DeleteById(entryId.ToString());
                        }
                    }
                }
            }

            var dbModel = this.modelConverter.GetDbModel(customRegionGroupModel);
            var addReturn = customRegionRepo.AddOrUpdate(dbModel);
            return this.modelConverter.GetModel(addReturn);
        }

        public bool DeleteById(string id)
        {
            var customRegionRepo = repositoryFactory.CreateCustomRegionGroupRepository();
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
    }
}