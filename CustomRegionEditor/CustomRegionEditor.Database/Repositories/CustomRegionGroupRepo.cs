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
    public class CustomRegionGroupRepo : ICustomRegionGroupRepository
    {
        public CustomRegionGroupRepo(IEagerLoader eagerLoader, ISessionManager sessionManager, ICustomRegionEntryRepository customRegionEntryRepository, ISubRegionRepo<AirportModel> airportRepo, ISubRegionRepo<CityModel> cityRepo, ISubRegionRepo<StateModel> stateRepo, ISubRegionRepo<CountryModel> countryRepo, ISubRegionRepo<RegionModel> regionRepo)
        {
            this.AirportRepo = airportRepo;
            this.StateRepo = stateRepo;
            this.CityRepo = cityRepo;
            this.CountryRepo = countryRepo;
            this.RegionRepo = regionRepo;
            this.EagerLoader = eagerLoader;
            this.SessionManager = sessionManager;
            this.CustomRegionEntryRepository = customRegionEntryRepository;
            _customRegionGroupList = new List<CustomRegionGroupModel>();
        }

        private ISessionManager SessionManager { get; }
        private IEagerLoader EagerLoader { get; }
        private ISubRegionRepo<AirportModel> AirportRepo { get; }
        private ISubRegionRepo<CityModel> CityRepo { get; }
        private ISubRegionRepo<StateModel> StateRepo { get; }
        private ISubRegionRepo<CountryModel> CountryRepo { get; }
        private ISubRegionRepo<RegionModel> RegionRepo { get; }

        private ICustomRegionEntryRepository CustomRegionEntryRepository { get; }

        private List<CustomRegionGroupModel> _customRegionGroupList;

        public void AddOrUpdate(CustomRegionGroupModel entity)
        {
            using (var dbSession = SessionManager.OpenSession())
            {
                dbSession.SaveOrUpdate(entity);
                dbSession.Flush();
            }
        }

        public void Delete(CustomRegionGroupModel entity)
        {
            if (entity == null) return;

            using (var dbSession = SessionManager.OpenSession())
            {
                dbSession.Delete(entity);
                dbSession.Flush();
            }
        }

        public List<CustomRegionGroupModel> List()
        {
            using (var dbSession = SessionManager.OpenSession())
            {
                _customRegionGroupList = dbSession.Query<CustomRegionGroupModel>().ToList();
            }

            return _customRegionGroupList;
        }

        public List<CustomRegionGroupModel> GetSearchResults(string searchTerm, string filter)
        {
            using (var dbSession = SessionManager.OpenSession())
            {
                var startsWith = new List<CustomRegionGroupModel>();
                var contains = new List<CustomRegionGroupModel>();
                if (searchTerm.Equals("-All", StringComparison.OrdinalIgnoreCase))
                {
                    return EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().ToList());
                }

                if (searchTerm.Equals("-Small", StringComparison.OrdinalIgnoreCase))
                {
                    return EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>()
                        .Where(a => a.CustomRegionEntries.Count < 25).ToList());
                }

                if (searchTerm.Equals("-Large", StringComparison.OrdinalIgnoreCase))
                {
                    return EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>()
                        .Where(a => a.CustomRegionEntries.Count >= 25).ToList());
                }

                if (searchTerm.Equals("-Rand", StringComparison.OrdinalIgnoreCase))
                {
                    var returnModels = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().ToList());
                    var rand = new Random();
                    return new List<CustomRegionGroupModel> { returnModels.ElementAt(rand.Next(returnModels.Count)) };
                }

                switch (filter)
                {
                    default:
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>()
                            .Where(s => s.Name.StartsWith(searchTerm)).ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                            !s.Name.StartsWith(searchTerm)
                            && (s.Name.Contains(searchTerm)
                                || s.Description.Contains(searchTerm))).ToList());
                        break;
                    case ("airport"):
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.Airport.Name)
                                    .Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.Airport.Name)
                                    .Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.Airport.Name)
                                    .Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("city"):
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.City.Name).Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.City.Name).Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.City.Name).Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("state"):
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.State.Name).Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.State.Name).Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.State.Name).Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("country"):
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.Country.Name)
                                    .Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.Country.Name)
                                    .Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.Country.Name)
                                    .Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("region"):
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.Region.Name).Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.Region.Name).Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.Region.Name)
                                    .Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                }
                var returnCustomRegionGroupList = startsWith.Concat(contains).ToList();
                return returnCustomRegionGroupList;
            }
        } //looks for any matches containing the search term, orders them by relevance 

        public CustomRegionGroupModel FindById(string id)
        {
            var customRegionGroupModel = new CustomRegionGroupModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                customRegionGroupModel = EagerLoader.LoadEntities(dbSession.Get<CustomRegionGroupModel>(Guid.Parse(id)));
            }
            return customRegionGroupModel;
        }

        public void DeleteById(string entryId)
        {
            var customRegionGroupModel = new CustomRegionGroupModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                customRegionGroupModel = dbSession.Get<CustomRegionGroupModel>(Guid.Parse(entryId));
            }
            Delete(customRegionGroupModel);
        }

        public void AddByType(string entry, string type, string regionId)
        {
            var validEntry = false;
            var customRegionGroupModel = new CustomRegionGroupModel();
            var customRegionEntryModel = new CustomRegionEntryModel
            {
                RowVersion = 1
            };
            using (var dbSession = SessionManager.OpenSession())
            {
                customRegionGroupModel = dbSession.Get<CustomRegionGroupModel>(Guid.Parse(regionId));
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
                    customRegionGroupModel = dbSession.Get<CustomRegionGroupModel>(Guid.Parse(regionId));
                    RemoveSubregions(customRegionGroupModel, customRegionEntryModel, type);
                    CheckForParents(customRegionGroupModel, type, customRegionEntryModel);
                    AddOrUpdate(customRegionGroupModel);
                }
            }
        } //adds a new entry to a group

        private void CheckForParents(CustomRegionGroupModel customRegionGroupModel, string type, CustomRegionEntryModel customRegionEntryModel)
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

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, CustomRegionEntryModel customRegionEntryModel, string type)
        {
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

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, CityModel cityModel)
        {
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Name == cityModel.Name).ToList());

            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Name != cityModel.Name).ToList();
        }

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, StateModel stateModel)
        {
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Name == stateModel.Name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.State?.Name == stateModel.Name).ToList());

            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Name != stateModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.State?.Name != stateModel.Name).ToList();
        }

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, CountryModel countryModel)
        {
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Country?.Name == countryModel.Name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Country?.Name == countryModel.Name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.Country?.Name == countryModel.Name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.State?.Country?.Name == countryModel.Name).ToList());

            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Country?.Name != countryModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Country?.Name != countryModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.Country?.Name != countryModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.State?.Country?.Name != countryModel.Name).ToList();
        }

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, RegionModel regionModel)
        {
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Country?.Region?.Name == regionModel.Name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Country?.Region?.Name == regionModel.Name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.Country?.Region?.Name == regionModel.Name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.State?.Country?.Region?.Name == regionModel.Name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.Country?.Region?.Name == regionModel.Name).ToList());

            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Country?.Region?.Name != regionModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Country?.Region?.Name != regionModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.Country?.Region?.Name != regionModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.State?.Country?.Region?.Name != regionModel.Name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Country?.Region?.Name != regionModel.Name).ToList();
        }

        public CustomRegionGroupModel AddNewRegion(CustomRegionGroupModel customRegionGroupModel)
        {
            using (var dbSession = SessionManager.OpenSession())
            {
                dbSession.SaveOrUpdate(customRegionGroupModel);
                dbSession.Flush();
                return EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().FirstOrDefault(a => a.Name == customRegionGroupModel.Name && a.Description == customRegionGroupModel.Description));
            }
        } //

        public void ChangeDetails(string name, string description, string regionId)
        {
            var customRegion = new CustomRegionGroupModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                customRegion = dbSession.Get<CustomRegionGroupModel>(Guid.Parse(regionId));
                if (name != "" && name != null)
                {
                    customRegion.Name = name;
                }
                if (description != "" && description != null)
                {
                    customRegion.Description = description;
                }
                dbSession.SaveOrUpdate(customRegion);
                dbSession.Flush();
            }
            //AddOrUpdate(customRegion);
        } //updates region group details

        public List<string> GetNames(string type)
        {
            var names = new List<string>();
            using (var dbSession = SessionManager.OpenSession())
            {
                switch (type)
                {
                    case "airport":
                        names = dbSession.Query<AirportModel>().Select(a => a.Name.ToUpper()).ToList();
                        return names;
                    case "city":
                        names = dbSession.Query<CityModel>().Select(a => a.Name.ToUpper()).ToList();
                        return names;
                    case "state":
                        names = dbSession.Query<StateModel>().Select(a => a.Name.ToUpper()).ToList();
                        return names;
                    case "country":
                        names = dbSession.Query<CountryModel>().Select(a => a.Name.ToUpper()).ToList();
                        return names;
                    case "region":
                        names = dbSession.Query<RegionModel>().Select(a => a.Name.ToUpper()).ToList();
                        return names;
                    default:
                        break;
                }
            }
            return null;
        }
    }
}