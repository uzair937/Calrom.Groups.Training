using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Database.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Utils;
using CustomRegionEditor.Database.NHibLazyLoader;
using CustomRegionEditor.Database.Interfaces;

namespace CustomRegionEditor.Database.Repositories
{
    public class CustomRegionGroupRepo : ICustomRegionGroupRepository
    {
        public CustomRegionGroupRepo(ILazyLoader lazyLoader, ISessionManager sessionManager, ICustomRegionEntryRepository customRegionEntryRepository)
        {
            LazyLoader = lazyLoader;
            this.SessionManager = sessionManager;
            this.CustomRegionEntryRepository = customRegionEntryRepository;
            _customRegionGroupList = new List<CustomRegionGroupModel>();
        }

        private ISessionManager SessionManager { get; }

        private ILazyLoader LazyLoader { get; }

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
                    return LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().ToList());
                }

                if (searchTerm.Equals("-Small", StringComparison.OrdinalIgnoreCase))
                {
                    return LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>()
                        .Where(a => a.CustomRegionEntries.Count < 25).ToList());
                }

                if (searchTerm.Equals("-Large", StringComparison.OrdinalIgnoreCase))
                {
                    return LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>()
                        .Where(a => a.CustomRegionEntries.Count >= 25).ToList());
                }

                if (searchTerm.Equals("-Rand", StringComparison.OrdinalIgnoreCase))
                {
                    var returnModels = LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().ToList());
                    var rand = new Random();
                    return new List<CustomRegionGroupModel> { returnModels.ElementAt(rand.Next(returnModels.Count)) };
                }

                switch (filter)
                {
                    case ("none"):
                        startsWith = LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>()
                            .Where(s => s.custom_region_name.StartsWith(searchTerm)).ToList());

                        contains = LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                            !s.custom_region_name.StartsWith(searchTerm)
                            && (s.custom_region_name.Contains(searchTerm)
                                || s.custom_region_description.Contains(searchTerm))).ToList());
                        break;
                    case ("airport"):
                        startsWith = LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.apt.airport_name)
                                    .Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.apt.airport_name)
                                    .Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.apt.airport_name)
                                    .Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("city"):
                        startsWith = LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.cty.city_name).Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.cty.city_name).Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.cty.city_name).Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("state"):
                        startsWith = LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.sta.state_name).Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.sta.state_name).Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.sta.state_name).Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("country"):
                        startsWith = LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.cnt.country_name)
                                    .Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.cnt.country_name)
                                    .Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.cnt.country_name)
                                    .Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                    case ("region"):
                        startsWith = LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                s.CustomRegionEntries.Select(a => a.reg.region_name).Any(w => w.StartsWith(searchTerm)))
                            .ToList());

                        contains = LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s =>
                                !s.CustomRegionEntries.Select(a => a.reg.region_name).Any(w => w.StartsWith(searchTerm))
                                && s.CustomRegionEntries.Select(a => a.reg.region_name)
                                    .Any(w => w.Contains(searchTerm)))
                            .ToList());
                        break;
                }

                _customRegionGroupList = startsWith.Concat(contains).ToList();
            }

            return _customRegionGroupList;
        } //looks for any matches containing the search term, orders them by relevance 

        public CustomRegionGroupModel GetFilteredResults(string countryName, string filter)
        {

            var crg = new CustomRegionGroupModel()
            {
                CustomRegionEntries = new List<CustomRegionEntryModel>()
            };
            switch (filter)
            {
                case "countryFilter":
                    //using (var dbSession = SessionManager.OpenSession())
                    //{
                    //    var crg = new CustomRegionGroupModel()
                    //    {
                    //        CustomRegionEntries = new List<CustomRegionEntryModel>()
                    //    };
                    //    var cities = dbSession.Query<CityModel>().Where(c => c.cnt.country_name == countryName).ToList();
                    //    var states = dbSession.Query<StateModel>().Where(s => s.cnt.country_name == countryName).ToList();
                        
                    //    foreach (CityModel city in cities)
                    //    {
                            
                    //    }
                    //    foreach (StateModel state in states)
                    //    {
                    //        crg.CustomRegionEntries.Add(new CustomRegionEntryModel()
                    //        {
                    //            sta = state
                    //        });
                    //    }
                    //    foreach (AirportModel airport in airports)
                    //    {
                    //        crg.CustomRegionEntries.Add(new CustomRegionEntryModel()
                    //        {
                    //            apt = airport
                    //        });
                    //    }
                    //}
                    break;

                case "cityFilter":
                    

                    break;

                default:
                    break;
            }

            return crg;
        }

        public List<CustomRegionEntryModel> GetCitySubRegions(CityModel city)
        {
            var CustomRegionEntries = new List<CustomRegionEntryModel>();

            using (var dbSession = SessionManager.OpenSession())
            {
                var airports = dbSession.Query<AirportModel>().Where(c => c.cty.cty_id == city.cty_id).ToList();
                
                foreach (var airport in airports)
                {
                    CustomRegionEntries.Add(new CustomRegionEntryModel()
                    {
                        apt = airport
                    });
                }
            }
            return CustomRegionEntries;
        }
        
        public List<CustomRegionEntryModel> GetStateSubRegions(StateModel state)
        {
            var CustomRegionEntries = new List<CustomRegionEntryModel>();

            using (var dbSession = SessionManager.OpenSession())
            {
                var cities = dbSession.Query<CityModel>().Where(c => c.sta.sta_id == state.sta_id).ToList();
                
                foreach (var city in cities)
                {
                    CustomRegionEntries.Add(new CustomRegionEntryModel()
                    {
                        cty = city
                    });
                    CustomRegionEntries.Concat(GetCitySubRegions(city));
                }
            }
            return CustomRegionEntries;
        }
        
        public List<CustomRegionEntryModel> GetCountrySubRegions(CountryModel country)
        {
            var CustomRegionEntries = new List<CustomRegionEntryModel>();

            using (var dbSession = SessionManager.OpenSession())
            {
                var cities = dbSession.Query<CityModel>().Where(c => c.cnt.cnt_id == country.cnt_id).ToList();
                foreach (var city in cities)
                {
                    CustomRegionEntries.Add(new CustomRegionEntryModel()
                    {
                        cty = city
                    });
                    CustomRegionEntries.Concat(GetCitySubRegions(city));
                }
                var states = new List<StateModel>();
                states.Concat(dbSession.Query<StateModel>().Where(s => s.cnt.cnt_id == country.cnt_id).ToList());
                foreach (var state in states)
                {
                    CustomRegionEntries.Add(new CustomRegionEntryModel()
                    {
                        sta = state
                    });
                    CustomRegionEntries.Concat(GetStateSubRegions(state));
                }
            }
            return CustomRegionEntries;
        }

        public CustomRegionGroupModel FindById(string id)
        {
            var customRegionGroupModel = new CustomRegionGroupModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                customRegionGroupModel = LazyLoader.LoadEntities(dbSession.Get<CustomRegionGroupModel>(Guid.Parse(id)));
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
                row_version = 1
            };
            using (var dbSession = SessionManager.OpenSession())
            {
                customRegionGroupModel = dbSession.Get<CustomRegionGroupModel>(Guid.Parse(regionId));
                customRegionEntryModel.crg = customRegionGroupModel;
                switch (type) //changed from if elses to a switch cus cleaner
                {
                    case "airport":
                        customRegionEntryModel.apt = GetAirport(entry); //needs to add a reference to the object for each
                        if (customRegionEntryModel.apt != null) validEntry = true;
                        break;
                    case "city":
                        customRegionEntryModel.cty = GetCity(entry);
                        if (customRegionEntryModel.cty != null) validEntry = true;
                        break;
                    case "state":
                        customRegionEntryModel.sta = GetState(entry);
                        if (customRegionEntryModel.sta != null) validEntry = true;
                        break;
                    case "country":
                        customRegionEntryModel.cnt = GetCountry(entry);
                        if (customRegionEntryModel.cnt != null) validEntry = true;
                        break;
                    case "region":
                        customRegionEntryModel.reg = GetRegion(entry);
                        if (customRegionEntryModel.reg != null) validEntry = true;
                        break;
                    default:
                        break;
                }
            }
            if (validEntry)
            {
                using (var dbSession = SessionManager.OpenSession())
                {
                    customRegionGroupModel = dbSession.Get<CustomRegionGroupModel>(Guid.Parse(regionId));
                    RemoveSubregions(customRegionGroupModel, customRegionEntryModel, type);
                    CheckForParents(customRegionGroupModel, type, customRegionEntryModel);
                }
                AddOrUpdate(customRegionGroupModel);
            }
        } //adds a new entry to a group

        private void CheckForParents(CustomRegionGroupModel customRegionGroupModel, string type, CustomRegionEntryModel customRegionEntryModel)
        {
            if (type == "airport" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.apt?.airport_name == customRegionEntryModel?.apt?.airport_name) == null)
            {
                if ((!customRegionGroupModel.CustomRegionEntries.Select(c => c.cty?.city_name).Contains(customRegionEntryModel.apt?.cty?.city_name) || customRegionEntryModel.apt?.cty?.city_name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.sta?.state_name).Contains(customRegionEntryModel.apt?.cty?.sta?.state_name) || customRegionEntryModel.apt?.cty?.sta?.state_name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.cnt?.country_name).Contains(customRegionEntryModel.apt?.cty?.cnt?.country_name) || customRegionEntryModel.apt?.cty?.sta?.cnt?.country_name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.cnt?.country_name).Contains(customRegionEntryModel.apt?.cty?.sta?.cnt?.country_name) || customRegionEntryModel.apt?.cty?.sta?.cnt?.country_name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.reg?.region_name).Contains(customRegionEntryModel.apt?.cty?.sta?.cnt?.reg?.region_name) || customRegionEntryModel.apt?.cty?.sta?.cnt?.reg?.region_name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.reg?.region_name).Contains(customRegionEntryModel.apt?.cty?.cnt?.reg?.region_name) || customRegionEntryModel.apt?.cty?.cnt?.reg?.region_name == null))
                {
                    customRegionGroupModel.CustomRegionEntries.Add(customRegionEntryModel);
                }
            }
            if (type == "city" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.cty?.city_name == customRegionEntryModel?.cty?.city_name) == null)
            {
                if ((!customRegionGroupModel.CustomRegionEntries.Select(c => c.sta?.state_name).Contains(customRegionEntryModel.cty?.sta?.state_name) || customRegionEntryModel.cty?.sta?.state_name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.cnt?.country_name).Contains(customRegionEntryModel.cty?.cnt?.country_name) || customRegionEntryModel.cty?.cnt?.country_name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.cnt?.country_name).Contains(customRegionEntryModel.cty?.sta?.cnt?.country_name) || customRegionEntryModel.cty?.sta?.cnt?.country_name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.reg?.region_name).Contains(customRegionEntryModel.cty?.sta?.cnt?.reg?.region_name) || customRegionEntryModel.cty?.sta?.cnt?.reg?.region_name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.reg?.region_name).Contains(customRegionEntryModel.cty?.cnt?.reg?.region_name) || customRegionEntryModel.cty?.cnt?.reg?.region_name == null))
                {
                    customRegionGroupModel.CustomRegionEntries.Add(customRegionEntryModel);
                }
            }
            if (type == "state" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.sta?.state_name == customRegionEntryModel?.sta?.state_name) == null)
            {
                if ((!customRegionGroupModel.CustomRegionEntries.Select(c => c.cnt?.country_name).Contains(customRegionEntryModel.sta?.cnt?.country_name) || customRegionEntryModel.sta?.cnt?.country_name == null)
                    && (!customRegionGroupModel.CustomRegionEntries.Select(c => c.reg?.region_name).Contains(customRegionEntryModel.sta?.cnt?.reg?.region_name) || customRegionEntryModel.sta?.cnt?.reg?.region_name == null))
                {
                    customRegionGroupModel.CustomRegionEntries.Add(customRegionEntryModel);
                }
            }
            if (type == "country" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.cnt?.country_name == customRegionEntryModel?.cnt?.country_name) == null)
            {
                if (!customRegionGroupModel.CustomRegionEntries.Select(c => c.reg?.region_name).Contains(customRegionEntryModel.cnt?.reg?.region_name) || customRegionEntryModel.cnt?.reg?.region_name == null)
                {
                    customRegionGroupModel.CustomRegionEntries.Add(customRegionEntryModel);
                }
            }
            if (type == "region" && customRegionGroupModel.CustomRegionEntries.FirstOrDefault(a => a?.reg?.region_name == customRegionEntryModel?.reg?.region_name) == null)
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
                    RemoveSubregions(customRegionGroupModel, customRegionEntryModel.cty);
                    break;
                case "state":
                    RemoveSubregions(customRegionGroupModel, customRegionEntryModel.sta);
                    break;
                case "country":
                    RemoveSubregions(customRegionGroupModel, customRegionEntryModel.cnt);
                    break;
                case "region":
                    RemoveSubregions(customRegionGroupModel, customRegionEntryModel.reg);
                    break;
            }

        }

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, CityModel cityModel)
        {
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.city_name == cityModel.city_name).ToList());

            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.city_name != cityModel.city_name).ToList();
        }

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, StateModel stateModel)
        {
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.sta?.state_name == stateModel.state_name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.cty?.sta?.state_name == stateModel.state_name).ToList());

            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.sta?.state_name != stateModel.state_name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.cty?.sta?.state_name != stateModel.state_name).ToList();
        }

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, CountryModel countryModel)
        {
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.sta?.cnt?.country_name == countryModel.country_name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.cnt?.country_name == countryModel.country_name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.cty?.cnt?.country_name == countryModel.country_name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.sta?.cnt?.country_name == countryModel.country_name).ToList());

            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.sta?.cnt?.country_name != countryModel.country_name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.cnt?.country_name != countryModel.country_name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.cty?.cnt?.country_name != countryModel.country_name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.sta?.cnt?.country_name != countryModel.country_name).ToList();
        }

        private void RemoveSubregions(CustomRegionGroupModel customRegionGroupModel, RegionModel regionModel)
        {
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.sta?.cnt?.reg?.region_name == regionModel.region_name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.cnt?.reg?.region_name == regionModel.region_name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.cty?.cnt?.reg?.region_name == regionModel.region_name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.sta?.cnt?.reg?.region_name == regionModel.region_name).ToList());
            CustomRegionEntryRepository.Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.cnt?.reg?.region_name == regionModel.region_name).ToList());

            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.sta?.cnt?.reg?.region_name != regionModel.region_name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.cnt?.reg?.region_name != regionModel.region_name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.cty?.cnt?.reg?.region_name != regionModel.region_name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.sta?.cnt?.reg?.region_name != regionModel.region_name).ToList();
            customRegionGroupModel.CustomRegionEntries = customRegionGroupModel.CustomRegionEntries.Where(a => a?.cnt?.reg?.region_name != regionModel.region_name).ToList();
        }

        public AirportModel GetAirport(string entry)
        {
            var airportModel = new AirportModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                airportModel = dbSession.Query<AirportModel>().FirstOrDefault(a => a.airport_name == (entry));
                if (airportModel == null) airportModel = dbSession.Query<AirportModel>().FirstOrDefault(a => a.apt_id == (entry));
                return LazyLoader.LoadEntities(airportModel);
            }

        } //searches for a matching airport

        public CityModel GetCity(string entry)
        {
            var cityModel = new CityModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                cityModel = dbSession.Query<CityModel>().FirstOrDefault(a => a.city_name == (entry));
                if (cityModel == null) cityModel = dbSession.Query<CityModel>().FirstOrDefault(a => a.cty_id == (entry));
                return LazyLoader.LoadEntities(cityModel);
            }

        } //searches for a matching city

        public StateModel GetState(string entry)
        {
            var stateModel = new StateModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                stateModel = dbSession.Query<StateModel>().FirstOrDefault(a => a.state_name == (entry));
                if (stateModel == null) stateModel = dbSession.Query<StateModel>().FirstOrDefault(a => a.sta_id == (entry));
                return LazyLoader.LoadEntities(stateModel);
            }

        } //searches for a matching state

        public CountryModel GetCountry(string entry)
        {
            var countryModel = new CountryModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                countryModel = dbSession.Query<CountryModel>().FirstOrDefault(a => a.country_name == (entry));
                if (countryModel == null) countryModel = dbSession.Query<CountryModel>().FirstOrDefault(a => a.cnt_id == (entry));
                return LazyLoader.LoadEntities(countryModel);
            }

        } //searches for a matching country

        public RegionModel GetRegion(string entry)
        {
            var regionModel = new RegionModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                regionModel = dbSession.Query<RegionModel>().FirstOrDefault(a => a.region_name == (entry));
                if (regionModel == null) regionModel = dbSession.Query<RegionModel>().FirstOrDefault(a => a.reg_id == (entry));
                return LazyLoader.LoadEntities(regionModel);
            }
        } //searches for a matching region

        public CustomRegionGroupModel AddNewRegion()
        {
            var customRegionGroupModel = new CustomRegionGroupModel
            {
                CustomRegionEntries = new List<CustomRegionEntryModel>(),
                custom_region_name = "Set Name",
                custom_region_description = "Set Description",
            };
            AddOrUpdate(customRegionGroupModel);
            using (var dbSession = SessionManager.OpenSession())
            {
                return LazyLoader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().FirstOrDefault(a => a.custom_region_name == "Set Name"));
            }
        } //Generates a new empty region

        public void ChangeDetails(string name, string description, string regionId)
        {
            var customRegion = new CustomRegionGroupModel();
            using (var dbSession = SessionManager.OpenSession())
            {
                customRegion = dbSession.Get<CustomRegionGroupModel>(Guid.Parse(regionId));
                if (name != "" & name != null) customRegion.custom_region_name = name;
                if (description != "" & description != null) customRegion.custom_region_description = description;
            }
            AddOrUpdate(customRegion);
        } //updates region group details

        public List<string> GetNames(string type)
        {
            var names = new List<string>();
            switch (type)
            {
                case "airport":
                    using (var dbSession = SessionManager.OpenSession())
                    {
                        names = dbSession.Query<AirportModel>().Select(a => a.airport_name.ToUpper()).ToList();
                    }
                    return names;
                case "city":
                    using (var dbSession = SessionManager.OpenSession())
                    {
                        names = dbSession.Query<CityModel>().Select(a => a.city_name.ToUpper()).ToList();
                    }
                    return names;
                case "state":
                    using (var dbSession = SessionManager.OpenSession())
                    {
                        names = dbSession.Query<StateModel>().Select(a => a.state_name.ToUpper()).ToList();
                    }
                    return names;
                case "country":
                    using (var dbSession = SessionManager.OpenSession())
                    {
                        names = dbSession.Query<CountryModel>().Select(a => a.country_name.ToUpper()).ToList();
                    }
                    return names;
                case "region":
                    using (var dbSession = SessionManager.OpenSession())
                    {
                        names = dbSession.Query<RegionModel>().Select(a => a.region_name.ToUpper()).ToList();
                    }
                    return names;
                default:
                    break;
            }
            return null;
        }
    }
}