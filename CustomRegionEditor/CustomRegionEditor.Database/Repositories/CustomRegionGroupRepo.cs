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
        public CustomRegionGroupRepo(IEagerLoader eagerLoader, ISessionManager sessionManager,
            ICustomRegionEntryRepository customRegionEntryRepository, ISubRegionRepo<AirportModel> airportRepo,
            ISubRegionRepo<CityModel> cityRepo, ISubRegionRepo<StateModel> stateRepo,
            ISubRegionRepo<CountryModel> countryRepo, ISubRegionRepo<RegionModel> regionRepo, ICustomRegionGroupTempRepo customRegionGroupTempRepo)
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
            this.CustomRegionGroupTempRepo = customRegionGroupTempRepo;
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
        private ICustomRegionGroupTempRepo CustomRegionGroupTempRepo { get; }

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
            var updatedList = new List<CustomRegionGroupModel>();
            using (var dbSession = SessionManager.OpenSession())
            {
                _customRegionGroupList = dbSession.Query<CustomRegionGroupModel>().ToList();

                foreach (var child in _customRegionGroupList)
                {
                    updatedList.Add(child);
                }
            }
            return updatedList;
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
                                s.CustomRegionEntries.Select(a => a.Airport.AirportName)
                                    .Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.Airport.AirportName)
                                    .Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.Airport.AirportName)
                                    .Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("city"):
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.City.CityName).Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.City.CityName).Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.City.CityName).Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("state"):
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.State.StateName).Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.State.StateName).Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.State.StateName)
                                    .Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("country"):
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.Country.CountryName)
                                    .Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.Country.CountryName)
                                    .Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.Country.CountryName)
                                    .Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("region"):
                        startsWith = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.Region.RegionName)
                                    .Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = EagerLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.Region.RegionName)
                                    .Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.Region.RegionName)
                                    .Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                }

                _customRegionGroupList = startsWith.Concat(contains).ToList();
            }

            return _customRegionGroupList;
        } //looks for any matches containing the search term, orders them by relevance 

        public CustomRegionGroupModel FindById(string id)
        {
            var customRegionGroupModel = new CustomRegionGroupModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                customRegionGroupModel =
                    EagerLoader.LoadEntities(dbSession.Get<CustomRegionGroupModel>(Guid.Parse(id)));
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

        public CustomRegionGroupModel AddByType(string entry, string type, string regionId)
        {
            var validEntry = false;
            var customRegionEntryModel = new CustomRegionEntryModel
            {
                RowVersion = 1
            };

            var customRegionGroupModel =
                this.CustomRegionGroupTempRepo.List().FirstOrDefault(a => a.Id == Guid.Parse(regionId)) ?? new CustomRegionGroupModel()
                {
                    CustomRegionEntries = new List<CustomRegionEntryModel>()
                };

            if (CustomRegionGroupTempRepo.List().Count > 0)
            {
                customRegionEntryModel.CustomRegionGroup = CustomRegionGroupTempRepo.List().FirstOrDefault(a => a.Id == Guid.Parse(regionId));
            }

            switch (type) //changed from if elses to a switch cus cleaner
            {
                case "airport":
                    customRegionEntryModel.Airport =
                        this.AirportRepo.FindByName(entry); //needs to add a reference to the object for each
                    if (customRegionEntryModel.Airport != null)
                    {
                        validEntry = true;
                        
                    }
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
                customRegionGroupModel.Name = FindById(regionId).Name;
                customRegionGroupModel.CustomRegionEntries.Add(customRegionEntryModel);
                RemoveSubregions(customRegionGroupModel, customRegionEntryModel, type);
                CheckForParents(customRegionGroupModel, type, customRegionEntryModel);
                CustomRegionGroupTempRepo.Add(customRegionGroupModel);
            }

            return customRegionGroupModel;
        }

        private void CheckForParents(CustomRegionGroupModel customRegionGroupModel, string type, CustomRegionEntryModel customRegionEntryModel)
        {
            if (type == "airport" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.Airport?.AirportName == customRegionEntryModel?.Airport?.AirportName) == null)
            {
                if ((!customRegionGroupModel.CustomRegionEntries.Select(c => c.City?.CityName).Contains(customRegionEntryModel.Airport?.City?.CityName) || customRegionEntryModel.Airport?.City?.CityName == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.State?.StateName).Contains(customRegionEntryModel.Airport?.City?.State?.StateName) || customRegionEntryModel.Airport?.City?.State?.StateName == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.CountryName).Contains(customRegionEntryModel.Airport?.City?.Country?.CountryName) || customRegionEntryModel.Airport?.City?.State?.Country?.CountryName == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.CountryName).Contains(customRegionEntryModel.Airport?.City?.State?.Country?.CountryName) || customRegionEntryModel.Airport?.City?.State?.Country?.CountryName == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.RegionName).Contains(customRegionEntryModel.Airport?.City?.State?.Country?.Region?.RegionName) || customRegionEntryModel.Airport?.City?.State?.Country?.Region?.RegionName == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.RegionName).Contains(customRegionEntryModel.Airport?.City?.Country?.Region?.RegionName) || customRegionEntryModel.Airport?.City?.Country?.Region?.RegionName == null))
                {
                    customRegionGroupModel.CustomRegionEntries.Add(customRegionEntryModel);
                }
            }
            if (type == "city" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.City?.CityName == customRegionEntryModel?.City?.CityName) == null)
            {
                if ((!customRegionGroupModel.CustomRegionEntries.Select(c => c.State?.StateName).Contains(customRegionEntryModel.City?.State?.StateName) || customRegionEntryModel.City?.State?.StateName == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.CountryName).Contains(customRegionEntryModel.City?.Country?.CountryName) || customRegionEntryModel.City?.Country?.CountryName == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.CountryName).Contains(customRegionEntryModel.City?.State?.Country?.CountryName) || customRegionEntryModel.City?.State?.Country?.CountryName == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.RegionName).Contains(customRegionEntryModel.City?.State?.Country?.Region?.RegionName) || customRegionEntryModel.City?.State?.Country?.Region?.RegionName == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.RegionName).Contains(customRegionEntryModel.City?.Country?.Region?.RegionName) || customRegionEntryModel.City?.Country?.Region?.RegionName == null))
                {
                    customRegionGroupModel.CustomRegionEntries.Add(customRegionEntryModel);
                }
            }
            if (type == "state" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.State?.StateName == customRegionEntryModel?.State?.StateName) == null)
            {
                if ((!customRegionGroupModel.CustomRegionEntries.Select(c => c.Country?.CountryName).Contains(customRegionEntryModel.State?.Country?.CountryName) || customRegionEntryModel.State?.Country?.CountryName == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.RegionName).Contains(customRegionEntryModel.State?.Country?.Region?.RegionName) || customRegionEntryModel.State?.Country?.Region?.RegionName == null))
                {
                    customRegionGroupModel.CustomRegionEntries.Add(customRegionEntryModel);
                }
            }
            if (type == "country" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.Country?.CountryName == customRegionEntryModel?.Country?.CountryName) == null)
            {
                if (!customRegionGroupModel.CustomRegionEntries.Select(c => c.Region?.RegionName).Contains(customRegionEntryModel.Country?.Region?.RegionName) || customRegionEntryModel.Country?.Region?.RegionName == null)
                {
                    customRegionGroupModel.CustomRegionEntries.Add(customRegionEntryModel);
                }
            }
            if (type == "region" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.Region?.RegionName == customRegionEntryModel?.Region?.RegionName) == null)
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
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.CityName == cityModel.CityName).ToList());

            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.CityName != cityModel.CityName).ToList();
        }

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, StateModel stateModel)
        {
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.StateName == stateModel.StateName).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.State?.StateName == stateModel.StateName).ToList());

            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.StateName != stateModel.StateName).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.State?.StateName != stateModel.StateName).ToList();
        }

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, CountryModel countryModel)
        {
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Country?.CountryName == countryModel.CountryName).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Country?.CountryName == countryModel.CountryName).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.Country?.CountryName == countryModel.CountryName).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.State?.Country?.CountryName == countryModel.CountryName).ToList());

            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Country?.CountryName != countryModel.CountryName).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Country?.CountryName != countryModel.CountryName).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.Country?.CountryName != countryModel.CountryName).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.State?.Country?.CountryName != countryModel.CountryName).ToList();
        }

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, RegionModel regionModel)
        {
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Country?.Region?.RegionName == regionModel.RegionName).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Country?.Region?.RegionName == regionModel.RegionName).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.Country?.Region?.RegionName == regionModel.RegionName).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.State?.Country?.Region?.RegionName == regionModel.RegionName).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.Country?.Region?.RegionName == regionModel.RegionName).ToList());

            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.State?.Country?.Region?.RegionName != regionModel.RegionName).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Airport?.City?.Country?.Region?.RegionName != regionModel.RegionName).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.City?.Country?.Region?.RegionName != regionModel.RegionName).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.State?.Country?.Region?.RegionName != regionModel.RegionName).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.Country?.Region?.RegionName != regionModel.RegionName).ToList();
        }

        public CustomRegionGroupModel AddNewRegion(string name, string description)
        {
            var customRegionGroupModel = new CustomRegionGroupModel
            {
                Id = new Guid()
            };

            if (!string.IsNullOrEmpty(name))
            {
                customRegionGroupModel.Name = name;
                customRegionGroupModel.Description = description;
                customRegionGroupModel.CustomRegionEntries = new List<CustomRegionEntryModel>();
                customRegionGroupModel.CustomRegionEntries = CustomRegionGroupTempRepo.List().FirstOrDefault(a => a.Name == name).CustomRegionEntries;
            }
            this.CustomRegionGroupTempRepo.Add(customRegionGroupModel);
            return customRegionGroupModel;
        } //Generates a new empty region

        public void ChangeDetails(CustomRegionGroupModel customRegionGroupModel)
        {
            using (var dbSession = SessionManager.OpenSession())
            {
                var customRegion = dbSession.Get<CustomRegionGroupModel>(customRegionGroupModel.Id);
                if (customRegion == null)
                {
                    customRegion = new CustomRegionGroupModel()
                    {
                        Name = customRegionGroupModel.Name,
                        Description = customRegionGroupModel.Description,
                        Id = customRegionGroupModel.Id,
                        CustomRegionEntries = new List<CustomRegionEntryModel>()
                    };
                }
                if (!string.IsNullOrEmpty(customRegionGroupModel.Name))
                {
                    customRegion.Name = customRegionGroupModel.Name;
                }
                if (!string.IsNullOrEmpty(customRegionGroupModel.Description))
                {
                    customRegion.Description = customRegionGroupModel.Description;
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
                        names = dbSession.Query<AirportModel>().Select(a => a.AirportName.ToUpper()).ToList();
                        return names;
                    case "city":
                        names = dbSession.Query<CityModel>().Select(a => a.CityName.ToUpper()).ToList();
                        return names;
                    case "state":
                        names = dbSession.Query<StateModel>().Select(a => a.StateName.ToUpper()).ToList();
                        return names;
                    case "country":
                        names = dbSession.Query<CountryModel>().Select(a => a.CountryName.ToUpper()).ToList();
                        return names;
                    case "region":
                        names = dbSession.Query<RegionModel>().Select(a => a.RegionName.ToUpper()).ToList();
                        return names;
                    default:
                        break;
                }
            }
            return null;
        }

        public void UpdateList(IList<CustomRegionEntryModel> list, string regionId)
        {
            this.CustomRegionGroupTempRepo.Update(list, regionId);
        }
    }
}