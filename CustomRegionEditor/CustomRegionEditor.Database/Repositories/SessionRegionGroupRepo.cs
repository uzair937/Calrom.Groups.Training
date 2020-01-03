using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Database.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Utils;
using CustomRegionEditor.Database.NHibernate;
using CustomRegionEditor.Database.Interfaces;

namespace CustomRegionEditor.Database.Repositories
{
    public class SessionRegionGroupRepo : ISessionRegionGroupRepository
    {
        public SessionRegionGroupRepo(ISessionManager sessionManager, ICustomRegionGroupRepository customRegionGroupRepository, ICustomRegionEntryRepository customRegionEntryRepository, ISubRegionRepo<AirportModel> airportRepo, ISubRegionRepo<CityModel> cityRepo, ISubRegionRepo<StateModel> stateRepo, ISubRegionRepo<CountryModel> countryRepo, ISubRegionRepo<RegionModel> regionRepo)
        {
            this.AirportRepo = airportRepo;
            this.StateRepo = stateRepo;
            this.CityRepo = cityRepo;
            this.CountryRepo = countryRepo;
            this.RegionRepo = regionRepo;
            this.SessionManager = sessionManager;
            this.CustomRegionGroupRepository = customRegionGroupRepository;
            this.CustomRegionEntryRepository = customRegionEntryRepository;
            _customRegionGroupModel = new CustomRegionGroupModel
            {
                CustomRegionEntries = new List<CustomRegionEntryModel>()
            };
        }

        private ISessionManager SessionManager { get; }
        private ISubRegionRepo<AirportModel> AirportRepo { get; }
        private ISubRegionRepo<CityModel> CityRepo { get; }
        private ISubRegionRepo<StateModel> StateRepo { get; }
        private ISubRegionRepo<CountryModel> CountryRepo { get; }
        private ISubRegionRepo<RegionModel> RegionRepo { get; }

        private ICustomRegionGroupRepository CustomRegionGroupRepository { get; }
        private ICustomRegionEntryRepository CustomRegionEntryRepository { get; }

        private CustomRegionGroupModel _customRegionGroupModel;

        private void CheckForParents(ref CustomRegionGroupModel customRegionGroupModel, string type, CustomRegionEntryModel customRegionEntryModel)
        {
            if (type == "airport" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.Airport?.Name == customRegionEntryModel?.Airport?.Name) == null)
            {
                if ((!customRegionGroupModel.CustomRegionEntries.Select(c => c.City?.Name).Contains(customRegionEntryModel.Airport?.City?.Name) || customRegionEntryModel.Airport?.City?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.State?.Name).Contains(customRegionEntryModel.Airport?.City?.State?.Name) || customRegionEntryModel.Airport?.City?.State?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.Name).Contains(customRegionEntryModel.Airport?.City?.Country?.Name) || customRegionEntryModel.Airport?.City?.State?.Country?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.Name).Contains(customRegionEntryModel.Airport?.City?.State?.Country?.Name) || customRegionEntryModel.Airport?.City?.State?.Country?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.Name).Contains(customRegionEntryModel.Airport?.City?.State?.Country?.Region?.Name) || customRegionEntryModel.Airport?.City?.State?.Country?.Region?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.Name).Contains(customRegionEntryModel.Airport?.City?.Country?.Region?.Name) || customRegionEntryModel.Airport?.City?.Country?.Region?.Name == null))
                {
                    customRegionGroupModel.CustomRegionEntries.Add(customRegionEntryModel);
                }
            }
            if (type == "city" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.City?.Name == customRegionEntryModel?.City?.Name) == null)
            {
                if ((!customRegionGroupModel.CustomRegionEntries.Select(c => c.State?.Name).Contains(customRegionEntryModel.City?.State?.Name) || customRegionEntryModel.City?.State?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.Name).Contains(customRegionEntryModel.City?.Country?.Name) || customRegionEntryModel.City?.Country?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.Name).Contains(customRegionEntryModel.City?.State?.Country?.Name) || customRegionEntryModel.City?.State?.Country?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.Name).Contains(customRegionEntryModel.City?.State?.Country?.Region?.Name) || customRegionEntryModel.City?.State?.Country?.Region?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.Name).Contains(customRegionEntryModel.City?.Country?.Region?.Name) || customRegionEntryModel.City?.Country?.Region?.Name == null))
                {
                    customRegionGroupModel.CustomRegionEntries.Add(customRegionEntryModel);
                }
            }
            if (type == "state" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.State?.Name == customRegionEntryModel?.State?.Name) == null)
            {
                if ((!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.Name).Contains(customRegionEntryModel.State?.Country?.Name) || customRegionEntryModel.State?.Country?.Name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.Name).Contains(customRegionEntryModel.State?.Country?.Region?.Name) || customRegionEntryModel.State?.Country?.Region?.Name == null))
                {
                    customRegionGroupModel.CustomRegionEntries.Add(customRegionEntryModel);
                }
            }
            if (type == "country" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.Country?.Name == customRegionEntryModel?.Country?.Name) == null)
            {
                if (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.Name).Contains(customRegionEntryModel.Country?.Region?.Name) || customRegionEntryModel.Country?.Region?.Name == null)
                {
                    customRegionGroupModel.CustomRegionEntries.Add(customRegionEntryModel);
                }
            }
            if (type == "region" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.Region?.Name == customRegionEntryModel?.Region?.Name) == null)
            {
                customRegionGroupModel.CustomRegionEntries.Add(customRegionEntryModel);
            }
        }

        private void RemoveSubregions(ref CustomRegionGroupModel customRegionGroupModel, CustomRegionEntryModel customRegionEntryModel, string type)
        {
            if (customRegionGroupModel.CustomRegionEntries != null)
            {
                switch (type)
                {
                    case "airport":
                        break;
                    case "city":
                        RemoveSubregions(ref customRegionGroupModel, customRegionEntryModel.City);
                        break;
                    case "state":
                        RemoveSubregions(ref customRegionGroupModel, customRegionEntryModel.State);
                        break;
                    case "country":
                        RemoveSubregions(ref customRegionGroupModel, customRegionEntryModel.Country);
                        break;
                    case "region":
                        RemoveSubregions(ref customRegionGroupModel, customRegionEntryModel.Region);
                        break;
                }
            }

        }

        private void RemoveSubregions(ref CustomRegionGroupModel customRegionGroupModel, CityModel cityModel)
        {
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Name != cityModel.Name).ToList();
        }

        private void RemoveSubregions(ref CustomRegionGroupModel customRegionGroupModel, StateModel stateModel)
        {
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Name != stateModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.State?.Name != stateModel.Name).ToList();
        }

        private void RemoveSubregions(ref CustomRegionGroupModel customRegionGroupModel, CountryModel countryModel)
        {
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Country?.Name != countryModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Country?.Name != countryModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.Country?.Name != countryModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.State?.Country?.Name != countryModel.Name).ToList();
        }

        private void RemoveSubregions(ref CustomRegionGroupModel customRegionGroupModel, RegionModel regionModel)
        {
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Country?.Region?.Name != regionModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Country?.Region?.Name != regionModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.Country?.Region?.Name != regionModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.State?.Country?.Region?.Name != regionModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Country?.Region?.Name != regionModel.Name).ToList();
        }

        public void AddByType(string entry, string type)
        {
            var validEntry = false;
            var customRegionGroupModel = _customRegionGroupModel;
            var customRegionEntryModel = new CustomRegionEntryModel
            {
                RowVersion = 1
            };
            using (var dbSession = SessionManager.OpenSession())
            {
                customRegionEntryModel.CustomRegionGroup = customRegionGroupModel;
                switch (type) //changed from if elses to a switch cus cleaner
                {
                    case "airport":
                        customRegionEntryModel.Airport = this.AirportRepo.FindByName(entry); //needs to add a reference to the object for each
                        if (customRegionEntryModel.Airport != null) validEntry = true;
                        break;
                    case "city":
                        customRegionEntryModel.City = this.CityRepo.FindByName(entry);
                        if (customRegionEntryModel.City != null) validEntry = true;
                        break;
                    case "state":
                        customRegionEntryModel.State = this.StateRepo.FindByName(entry);
                        if (customRegionEntryModel.State != null) validEntry = true;
                        break;
                    case "country":
                        customRegionEntryModel.Country = this.CountryRepo.FindByName(entry);
                        if (customRegionEntryModel.Country != null) validEntry = true;
                        break;
                    case "region":
                        customRegionEntryModel.Region = this.RegionRepo.FindByName(entry);
                        if (customRegionEntryModel.Region != null) validEntry = true;
                        break;
                    default:
                        break; //replace switch with a new view model and send all data across (three params too much)
                }

                if (validEntry)
                {
                    if (customRegionGroupModel.CustomRegionEntries == null)
                    {
                        customRegionGroupModel.CustomRegionEntries = new List<CustomRegionEntryModel>
                        {
                            customRegionEntryModel
                        };
                    }
                    else
                    {
                        RemoveSubregions(ref customRegionGroupModel, customRegionEntryModel, type);
                        CheckForParents(ref customRegionGroupModel, type, customRegionEntryModel);
                    }
                    _customRegionGroupModel = customRegionGroupModel;
                }
            }
        } //adds a new entry to a group

        public CustomRegionGroupModel SaveToDatabase(CustomRegionGroupModel customRegionGroupModel)
        {
            var sessionId = customRegionGroupModel.Id;
            if (!(sessionId == null || sessionId == Guid.Empty))
            {
                var oldModel = CustomRegionGroupRepository.FindById(sessionId.ToString());
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
                            this.CustomRegionEntryRepository.DeleteById(entryId.ToString());
                        }
                    }
                }
            }
            _customRegionGroupModel = CustomRegionGroupRepository.AddNewRegion(customRegionGroupModel);
            return _customRegionGroupModel;
        }

        public void ClearSession()
        {
            _customRegionGroupModel = new CustomRegionGroupModel
            {
                CustomRegionEntries = new List<CustomRegionEntryModel>()
            };
        }

        public CustomRegionGroupModel GetSessionRegion()
        {
            return _customRegionGroupModel;
        }

        public void SetSessionRegion(CustomRegionGroupModel customRegionGroupModel)
        {
            _customRegionGroupModel = customRegionGroupModel;
        }
    }
}