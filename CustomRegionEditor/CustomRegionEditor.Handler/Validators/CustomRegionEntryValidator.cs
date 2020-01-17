using CustomRegionEditor.Database.Factories;
using CustomRegionEditor.Handler.Interfaces;
using CustomRegionEditor.Models;
using NHibernate;
using System;
using System.Collections.Generic;

namespace CustomRegionEditor.Handler.Validators
{
    public class CustomRegionEntryValidator
    {
        private readonly ISession Session;
        private readonly IRepositoryFactory RepositoryFactory;
        private readonly IModelConverter ModelConverter;

        public CustomRegionEntryValidator(ISession session, IRepositoryFactory repositoryFactory, IModelConverter modelConverter)
        {
            this.Session = session;
            this.RepositoryFactory = repositoryFactory;
            this.ModelConverter = modelConverter;
        }

        public ValidationModel IsValid(CustomRegionGroupModel customRegion)
        {
            var validationModel = new ValidationModel();
            var validatedEntries = new List<CustomRegionEntryModel>();

            foreach (var entry in customRegion.CustomRegionEntries)
            {
                if (entry.Id == Guid.Empty)
                {
                    var validEntryModel = ExistingEntryCheck(entry);
                    if (validEntryModel.ValidEntry)
                    {
                        validatedEntries.Add(entry);
                    }
                }
                else
                {
                    var validEntryModel = ValidateGuid(entry);
                    if (validEntryModel.ValidEntry)
                    {
                        validatedEntries.Add(entry);
                    }
                }
            }

            if (validatedEntries.Count != customRegion.CustomRegionEntries.Count)
            {
                validationModel.Errors.Add(new ErrorModel { Message = "Mismatch in custom regions" });
            }

            return validationModel;
        }

        private ValidEntryModel ExistingEntryCheck(CustomRegionEntryModel customRegionEntry)
        {
            var validEntryModel = new ValidEntryModel();
            var type = customRegionEntry.GetLocationType();
            switch (type)
            {
                case "region":
                    var regionRepo = RepositoryFactory.CreateRegionRepository(this.Session);
                    customRegionEntry.Region = this.ModelConverter.GetModel(regionRepo.Find(customRegionEntry.LocationName));
                    if (customRegionEntry.Region != null) validEntryModel.ValidEntry = true;
                    break;

                case "country":
                    var countryRepo = RepositoryFactory.CreateCountryRepository(this.Session);
                    customRegionEntry.Country = this.ModelConverter.GetModel(countryRepo.Find(customRegionEntry.LocationName));
                    if (customRegionEntry.Country != null) validEntryModel.ValidEntry = true;
                    break;

                case "state":
                    var stateRepo = RepositoryFactory.CreateStateRepository(this.Session);
                    customRegionEntry.State = this.ModelConverter.GetModel(stateRepo.Find(customRegionEntry.LocationName));
                    if (customRegionEntry.State != null) validEntryModel.ValidEntry = true;
                    break;

                case "city":
                    var cityRepo = RepositoryFactory.CreateCityRepository(this.Session);
                    customRegionEntry.City = this.ModelConverter.GetModel(cityRepo.Find(customRegionEntry.LocationName));
                    if (customRegionEntry.City != null) validEntryModel.ValidEntry = true;
                    break;
                case "airport":
                    var airportRepo = RepositoryFactory.CreateAirportRepository(this.Session);
                    customRegionEntry.Airport = this.ModelConverter.GetModel(airportRepo.Find(customRegionEntry.LocationName));
                    if (customRegionEntry.Airport != null) validEntryModel.ValidEntry = true;
                    break;
                default:
                    validEntryModel.Error.Message = "Cannot find type of " + customRegionEntry.LocationName;
                    validEntryModel.Error.Warning = true;
                    validEntryModel.ValidEntry = false;
                    break;
            }

            return validEntryModel;
        }

        public ValidEntryModel CheckNewEntry(string entry, string type)
        {
            var validEntryModel = new ValidEntryModel();
            var customRegionEntry = new CustomRegionEntryModel();
            var typeError = false;
            
            if (entry.Contains(","))
            {
                var index = entry.IndexOf(",");
                entry = entry.Substring(0, index);
            }

            switch (type)
            {
                case "region":
                    var regionRepo = RepositoryFactory.CreateRegionRepository(this.Session);
                    customRegionEntry.Region = this.ModelConverter.GetModel(regionRepo.Find(entry));
                    if (customRegionEntry.Region != null) validEntryModel.ValidEntry = true;
                    break;

                case "country":
                    var countryRepo = RepositoryFactory.CreateCountryRepository(this.Session);
                    customRegionEntry.Country = this.ModelConverter.GetModel(countryRepo.Find(entry));
                    if (customRegionEntry.Country != null) validEntryModel.ValidEntry = true;
                    break;

                case "state":
                    var stateRepo = RepositoryFactory.CreateStateRepository(this.Session);
                    customRegionEntry.State = this.ModelConverter.GetModel(stateRepo.Find(entry));
                    if (customRegionEntry.State != null) validEntryModel.ValidEntry = true;
                    break;

                case "city":
                    var cityRepo = RepositoryFactory.CreateCityRepository(this.Session);
                    customRegionEntry.City = this.ModelConverter.GetModel(cityRepo.Find(entry));
                    if (customRegionEntry.City != null) validEntryModel.ValidEntry = true;
                    break;
                case "airport":
                    var airportRepo = RepositoryFactory.CreateAirportRepository(this.Session);
                    customRegionEntry.Airport = this.ModelConverter.GetModel(airportRepo.Find(entry));
                    if (customRegionEntry.Airport != null) validEntryModel.ValidEntry = true;
                    break;
                default:
                    validEntryModel.Error.Message = "Cannot find type of " + entry;
                    validEntryModel.Error.Warning = true;
                    validEntryModel.ValidEntry = false;
                    typeError = true;
                    break;
            }

            if (!typeError && !validEntryModel.ValidEntry)
            {
                validEntryModel.Error.Message = "Cannot find entry " + entry;
                validEntryModel.Error.Warning = true;
            }

            validEntryModel.CustomRegionEntryModel = customRegionEntry;
            return validEntryModel;
        }
    
        public ValidEntryModel ValidateGuid(CustomRegionEntryModel customRegionEntry)
        {
            var entryRepo = this.RepositoryFactory.CreateCustomRegionEntryRepository(Session);
            var validEntryModel = new ValidEntryModel();

            var foundEntry = entryRepo.FindById(customRegionEntry.Id);

            if (foundEntry != null)
            {
                validEntryModel.ValidEntry = true;
            }
            else
            {
                validEntryModel.Error.Message = "Cannot find entry " + customRegionEntry.LocationName;
                validEntryModel.Error.Warning = true;
                validEntryModel.ValidEntry = false;
            }

            return validEntryModel;
        }
    }
}
