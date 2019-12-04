using CustomRegionEditor.Database.Models;
using CustomRegionEditor.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegionEditor.Database
{
    public class CustomRegionRepo : IRepository<CustomRegionGroupModel>
    {
        private static CustomRegionRepo Instance = null;

        private static LazyLoader Loader = null;

        private static readonly object Padlock = new object();

        public static CustomRegionRepo GetInstance
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
                            Instance = new CustomRegionRepo();
                        }
                    }
                }
                return Instance;
            }
        }

        private List<CustomRegionGroupModel> _customRegionGroupList;

        public CustomRegionRepo()
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
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                dbSession.Delete(entity);
                dbSession.Flush();
            }
        }

        public void Delete(CustomRegionEntryModel entity)
        {
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

        public List<CustomRegionGroupModel> GetSearchResults(string searchTerm)
        {
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                _customRegionGroupList = Loader.LoadEntities(dbSession.Query<CustomRegionGroupModel>().Where(s => s.custom_region_name.Contains(searchTerm) || s.custom_region_description.Contains(searchTerm)).ToList());
            }
            return _customRegionGroupList;
        } //looks for any matches containing the search term

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

        public void DeleteEntry(string entryId, string regionId)
        {
            var customRegionGroupModel = new CustomRegionGroupModel();
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
                customRegionGroupModel = dbSession.Get<CustomRegionGroupModel>(Guid.Parse(regionId));
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
                if (validEntry) customRegionGroupModel.CustomRegionEntries.Add(customRegionEntryModel);
            }
            if (validEntry)
            {
                AddOrUpdate(customRegionGroupModel);
            }
        } //adds a new entry to a group

        public AirportModel GetAirport(string entry)
        {
            var airportModel = new AirportModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                airportModel = dbSession.Query<AirportModel>().FirstOrDefault(a => a.airport_name == (entry));
            }
            return airportModel;
        } //searches for a matching airport

        public CityModel GetCity(string entry)
        {
            var cityModel = new CityModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                cityModel = dbSession.Query<CityModel>().FirstOrDefault(a => a.city_name == (entry));
            }
            return cityModel;
        } //searches for a matching city

        public StateModel GetState(string entry)
        {
            var stateModel = new StateModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                stateModel = dbSession.Query<StateModel>().FirstOrDefault(a => a.state_name == (entry));
            }
            return stateModel;
        } //searches for a matching state

        public CountryModel GetCountry(string entry)
        {
            var countryModel = new CountryModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                countryModel = dbSession.Query<CountryModel>().FirstOrDefault(a => a.country_name == (entry));
            }
            return countryModel;
        } //searches for a matching country

        public RegionModel GetRegion(string entry)
        {
            var regionModel = new RegionModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                regionModel = dbSession.Query<RegionModel>().FirstOrDefault(a => a.region_name == (entry));
            }
            return regionModel;
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
                        names = dbSession.Query<AirportModel>().Select(a => a.airport_name).ToList();
                    }
                    return names;
                case "city":
                    using (var dbSession = NHibernateHelper.OpenSession())
                    {
                        names = dbSession.Query<CityModel>().Select(a => a.city_name).ToList();
                    }
                    return names;
                case "state":
                    using (var dbSession = NHibernateHelper.OpenSession())
                    {
                        names = dbSession.Query<StateModel>().Select(a => a.state_name).ToList();
                    }
                    return names;
                case "country":
                    using (var dbSession = NHibernateHelper.OpenSession())
                    {
                        names = dbSession.Query<CountryModel>().Select(a => a.country_name).ToList();
                    }
                    return names;
                case "region":
                    using (var dbSession = NHibernateHelper.OpenSession())
                    {
                        names = dbSession.Query<RegionModel>().Select(a => a.region_name).ToList();
                    }
                    return names;
                default:
                    break;
            }
            return null;
        }
    } //searches for a matching airport
}

