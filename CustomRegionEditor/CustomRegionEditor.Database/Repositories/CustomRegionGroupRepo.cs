using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database
{
    public class CustomRegionGroupRepo : IRepository<CustomRegionGroupModel>
    {
        private static CustomRegionGroupRepo Instance = null;

        private static LazyLoader Loader = null;

        private static readonly object Padlock = new object();

        public static CustomRegionGroupRepo GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (Padlock)
                    {
                        if (Instance == null)
                        {
                            Loader = new LazyLoader();
                            Instance = new CustomRegionGroupRepo();
                        }
                    }
                }
                return Instance;
            }
        }

        private List<CustomRegionGroupModel> _customRegionGroupList;

        public CustomRegionGroupRepo()
        {
            _customRegionGroupList = new List<CustomRegionGroupModel>();
        }

        public void AddOrUpdate(CustomRegionGroupModel entity)
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                dbSession.SaveOrUpdate(entity);
                dbSession.Flush();
            }
        }

        public void Delete(CustomRegionGroupModel entity)
        {
            if (entity == null) return;
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                dbSession.Delete(entity);
                dbSession.Flush();
            }
        }

        public void Delete(List<CustomRegionEntryModel> entities)
        {
            foreach (var entity in entities)
            {
                DeleteEntry(entity.cre_id.ToString());
            }
        }

        public void Delete(CustomRegionEntryModel entity)
        {
            if (entity == null) return;
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                dbSession.Delete(entity);
                dbSession.Flush();
            }
        }

        public List<CustomRegionGroupModel> List()
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                _customRegionGroupList = dbSession.Query<CustomRegionGroupModel>().ToList();
            }
            return _customRegionGroupList;
        }

        public List<CustomRegionGroupModel> GetSearchResults(string searchTerm, string filter)
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                var startsWith = new List<CustomRegionGroupModel>();
                var contains = new List<CustomRegionGroupModel>();
                if (searchTerm == "-All")
                {
                    return Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().ToList());
                }
                else if (searchTerm == "-Small")
                {
                    return Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(a => a.CustomRegionEntries.Count < 25).ToList());
                }
                else if (searchTerm == "-Large")
                {
                    return Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(a => a.CustomRegionEntries.Count >= 25).ToList());
                }
                else if (searchTerm == "-Rand")
                {
                    var returnModels = Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().ToList());
                    var rand = new Random();
                    return new List<CustomRegionGroupModel> { returnModels.ElementAt(rand.Next(returnModels.Count)) };
                }
                switch (filter)
                {
                    case ("none"):
                        startsWith = Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s => s.custom_region_name.StartsWith(searchTerm)).ToList());

                        contains = Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s => !s.custom_region_name.StartsWith(searchTerm)
                                                                                                          && (s.custom_region_name.Contains(searchTerm)
                                                                                                          || s.custom_region_description.Contains(searchTerm))).ToList());
                        break;
                    case ("airport"):
                        startsWith = Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s => s.CustomRegionEntries.Select(a => a.apt.airport_name).Any(w => w.StartsWith(searchTerm))).ToList());

                        contains = Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s => !s.CustomRegionEntries.Select(a => a.apt.airport_name).Any(w => w.StartsWith(searchTerm))
                                                                                                          && s.CustomRegionEntries.Select(a => a.apt.airport_name).Any(w => w.Contains(searchTerm))).ToList());
                        break;
                    case ("city"):
                        startsWith = Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s => s.CustomRegionEntries.Select(a => a.cty.city_name).Any(w => w.StartsWith(searchTerm))).ToList());

                        contains = Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s => !s.CustomRegionEntries.Select(a => a.cty.city_name).Any(w => w.StartsWith(searchTerm))
                                                                                                          && s.CustomRegionEntries.Select(a => a.cty.city_name).Any(w => w.Contains(searchTerm))).ToList());
                        break;
                    case ("state"):
                        startsWith = Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s => s.CustomRegionEntries.Select(a => a.sta.state_name).Any(w => w.StartsWith(searchTerm))).ToList());

                        contains = Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s => !s.CustomRegionEntries.Select(a => a.sta.state_name).Any(w => w.StartsWith(searchTerm))
                                                                                                          && s.CustomRegionEntries.Select(a => a.sta.state_name).Any(w => w.Contains(searchTerm))).ToList());
                        break;
                    case ("country"):
                        startsWith = Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s => s.CustomRegionEntries.Select(a => a.cnt.country_name).Any(w => w.StartsWith(searchTerm))).ToList());

                        contains = Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s => !s.CustomRegionEntries.Select(a => a.cnt.country_name).Any(w => w.StartsWith(searchTerm))
                                                                                                          && s.CustomRegionEntries.Select(a => a.cnt.country_name).Any(w => w.Contains(searchTerm))).ToList());
                        break;
                    case ("region"):
                        startsWith = Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s => s.CustomRegionEntries.Select(a => a.reg.region_name).Any(w => w.StartsWith(searchTerm))).ToList());

                        contains = Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s => !s.CustomRegionEntries.Select(a => a.reg.region_name).Any(w => w.StartsWith(searchTerm))
                                                                                                          && s.CustomRegionEntries.Select(a => a.reg.region_name).Any(w => w.Contains(searchTerm))).ToList());
                        break;
                }
                _customRegionGroupList = startsWith.Concat(contains).ToList();
            }
            return _customRegionGroupList;
        } //looks for any matches containing the search term, orders them by relevance 

        public CustomRegionGroupModel FindById(string id)
        {
            var customRegionGroupModel = new CustomRegionGroupModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                customRegionGroupModel = Loader.LoadEntities(dbSession.Get<CustomRegionGroupModel>(Guid.Parse(id)));
            }
            return customRegionGroupModel;
        }

        public void DeleteById(string id)
        {
            var customRegionGroupModel = new CustomRegionGroupModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                customRegionGroupModel = dbSession.Get<CustomRegionGroupModel>(Guid.Parse(id));
            }
            Delete(customRegionGroupModel);
        }

        public void DeleteEntry(string entryId)
        {
            var customRegionEntryModel = new CustomRegionEntryModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                customRegionEntryModel = dbSession.Get<CustomRegionEntryModel>(Guid.Parse(entryId));
            }
            Delete(customRegionEntryModel);
        }

        public void AddByType(string entry, string type, string regionId)
        {
            var validEntry = false;
            var customRegionGroupModel = new CustomRegionGroupModel();
            var customRegionEntryModel = new CustomRegionEntryModel
            {
                apt = null,
                cty = null,
                sta = null,
                cnt = null,
                reg = null,
                row_version = 1
            };
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                customRegionGroupModel = Loader.LoadEntities(dbSession.Get<CustomRegionGroupModel>(Guid.Parse(regionId)));
                customRegionEntryModel.crg = customRegionGroupModel;
                if (type == "airport")
                {
                    customRegionEntryModel.apt = GetAirport(entry); //needs to add a reference to the object for each
                    if (customRegionEntryModel.apt != null) validEntry = true;
                }
                else if (type == "city")
                {
                    customRegionEntryModel.cty = GetCity(entry);
                    if (customRegionEntryModel.cty != null) validEntry = true;
                }
                else if (type == "state")
                {
                    customRegionEntryModel.sta = GetState(entry);
                    if (customRegionEntryModel.sta != null) validEntry = true;
                }
                else if (type == "country")
                {
                    customRegionEntryModel.cnt = GetCountry(entry);
                    if (customRegionEntryModel.cnt != null) validEntry = true;
                }
                else if (type == "region")
                {
                    customRegionEntryModel.reg = GetRegion(entry);
                    if (customRegionEntryModel.reg != null) validEntry = true;
                }
                if (validEntry)
                {
                    removeChildRegions(customRegionGroupModel, customRegionEntryModel, type);
                }
            }
            if (validEntry)
            {
                using (var dbSession = NHibernateHelper.OpenSession())
                {
                    customRegionGroupModel = dbSession.Get<CustomRegionGroupModel>(Guid.Parse(regionId));
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
                AddOrUpdate(customRegionGroupModel);
            }
        } //adds a new entry to a group

        private void removeChildRegions(CustomRegionGroupModel customRegionGroupModel, CustomRegionEntryModel customRegionEntryModel, string type)
        {
            switch (type)
            {
                case "airport":
                    break;
                case "city":
                    removeChildRegions(customRegionGroupModel, customRegionEntryModel.cty);
                    break;
                case "state":
                    removeChildRegions(customRegionGroupModel, customRegionEntryModel.sta);
                    break;
                case "country":
                    removeChildRegions(customRegionGroupModel, customRegionEntryModel.cnt);
                    break;
                case "region":
                    removeChildRegions(customRegionGroupModel, customRegionEntryModel.reg);
                    break;
            }
        }

        private void removeChildRegions(CustomRegionGroupModel customRegionGroupModel, CityModel cityModel)
        {
            Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.city_name == cityModel.city_name).ToList());
        }

        private void removeChildRegions(CustomRegionGroupModel customRegionGroupModel, StateModel stateModel)
        {
            Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.sta?.state_name == stateModel.state_name).ToList());
            Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.cty?.sta?.state_name == stateModel.state_name).ToList());
        }

        private void removeChildRegions(CustomRegionGroupModel customRegionGroupModel, CountryModel countryModel)
        {
            Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.sta?.cnt?.country_name == countryModel.country_name).ToList());
            Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.cnt?.country_name == countryModel.country_name).ToList());
            Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.cty?.cnt?.country_name == countryModel.country_name).ToList());
            Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.sta?.cnt?.country_name == countryModel.country_name).ToList());
        }

        private void removeChildRegions(CustomRegionGroupModel customRegionGroupModel, RegionModel regionModel)
        {
            Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.sta?.cnt?.reg?.region_name == regionModel.region_name).ToList());
            Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.apt?.cty?.cnt?.reg?.region_name == regionModel.region_name).ToList());
            Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.cty?.cnt?.reg?.region_name == regionModel.region_name).ToList());
            Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.sta?.cnt?.reg?.region_name == regionModel.region_name).ToList());
            Delete(customRegionGroupModel.CustomRegionEntries.Where(a => a?.cnt?.reg?.region_name == regionModel.region_name).ToList());
        }

        public AirportModel GetAirport(string entry)
        {
            var airportModel = new AirportModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                airportModel = dbSession.Query<AirportModel>().FirstOrDefault(a => a.airport_name == (entry));
                if (airportModel == null) airportModel = dbSession.Query<AirportModel>().FirstOrDefault(a => a.apt_id == (entry));
                return Loader.LoadEntities(airportModel);
            }
        } //searches for a matching airport

        public CityModel GetCity(string entry)
        {
            var cityModel = new CityModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                cityModel = dbSession.Query<CityModel>().FirstOrDefault(a => a.city_name == (entry));
                if (cityModel == null) cityModel = dbSession.Query<CityModel>().FirstOrDefault(a => a.cty_id == (entry));
                return Loader.LoadEntities(cityModel);
            }
        } //searches for a matching city

        public StateModel GetState(string entry)
        {
            var stateModel = new StateModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                stateModel = dbSession.Query<StateModel>().FirstOrDefault(a => a.state_name == (entry));
                if (stateModel == null) stateModel = dbSession.Query<StateModel>().FirstOrDefault(a => a.sta_id == (entry));
                return Loader.LoadEntities(stateModel);
            }
        } //searches for a matching state

        public CountryModel GetCountry(string entry)
        {
            var countryModel = new CountryModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                countryModel = dbSession.Query<CountryModel>().FirstOrDefault(a => a.country_name == (entry));
                if (countryModel == null) countryModel = dbSession.Query<CountryModel>().FirstOrDefault(a => a.cnt_id == (entry));
                return Loader.LoadEntities(countryModel);
            }
        } //searches for a matching country

        public RegionModel GetRegion(string entry)
        {
            var regionModel = new RegionModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                regionModel = dbSession.Query<RegionModel>().FirstOrDefault(a => a.region_name == (entry));
                if (regionModel == null) regionModel = dbSession.Query<RegionModel>().FirstOrDefault(a => a.reg_id == (entry));
                return Loader.LoadEntities(regionModel);
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
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                return Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().FirstOrDefault(a => a.custom_region_name == "Set Name"));
            }
        } //Generates a new empty region

        public void ChangeDetails(string name, string description, string regionId)
        {
            var customRegion = new CustomRegionGroupModel();
            using (var dbSession = NHibernateHelper.OpenSession())
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
                    using (var dbSession = NHibernateHelper.OpenSession())
                    {
                        names = dbSession.Query<AirportModel>().Select(a => a.airport_name.ToUpper()).ToList();
                    }
                    return names;
                case "city":
                    using (var dbSession = NHibernateHelper.OpenSession())
                    {
                        names = dbSession.Query<CityModel>().Select(a => a.city_name.ToUpper()).ToList();
                    }
                    return names;
                case "state":
                    using (var dbSession = NHibernateHelper.OpenSession())
                    {
                        names = dbSession.Query<StateModel>().Select(a => a.state_name.ToUpper()).ToList();
                    }
                    return names;
                case "country":
                    using (var dbSession = NHibernateHelper.OpenSession())
                    {
                        names = dbSession.Query<CountryModel>().Select(a => a.country_name.ToUpper()).ToList();
                    }
                    return names;
                case "region":
                    using (var dbSession = NHibernateHelper.OpenSession())
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

