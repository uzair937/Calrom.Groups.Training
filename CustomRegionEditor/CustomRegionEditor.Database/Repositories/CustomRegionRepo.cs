using CustomRegionEditor.Database.Models;
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
                _customRegionGroupList = dbSession.Query<CustomRegionGroupModel>().Where(s => s.custom_region_name.Contains(searchTerm) || s.custom_region_description.Contains(searchTerm)).ToList();
            }
            return _customRegionGroupList;
        }

        public CustomRegionGroupModel FindById(string id)
        {
            var customRegionGroupModel = new CustomRegionGroupModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                customRegionGroupModel = dbSession.Get<CustomRegionGroupModel>(id);
            }
            return customRegionGroupModel;
        }

        public void DeleteById(string id)
        {
            var customRegionGroupModel = new CustomRegionGroupModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                customRegionGroupModel = dbSession.Get<CustomRegionGroupModel>(id);
            }
            Delete(customRegionGroupModel);
        }

        public void DeleteEntry(string entryId, string regionId)
        {
            var customRegionGroupModel = new CustomRegionGroupModel();
            var customRegionEntryModel = new CustomRegionEntryModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                customRegionGroupModel = dbSession.Get<CustomRegionGroupModel>(regionId);
                customRegionEntryModel = dbSession.Get<CustomRegionEntryModel>(entryId);
                customRegionGroupModel.CustomRegionEntries.Remove(customRegionEntryModel);
            }
            AddOrUpdate(customRegionGroupModel);
        }

        public void AddByType(string entry, string type, string regionId)
        {
            var customRegionGroupModel = new CustomRegionGroupModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                customRegionGroupModel = dbSession.Get<CustomRegionGroupModel>(regionId);
            }
            var customRegionEntryModel = new CustomRegionEntryModel
            {
                Airport = null,
                City = null,
                State = null,
                Country = null,
                Region = null,
            };
            if (type == "airport") customRegionEntryModel.Airport = GetAirport(entry); //needs to add a reference to the object for each
            else if (type == "city") customRegionEntryModel.City = GetCity(entry);
            else if (type == "state") customRegionEntryModel.State = GetState(entry);
            else if (type == "country") customRegionEntryModel.Country = GetCountry(entry);
            else if (type == "region") customRegionEntryModel.Region = GetRegion(entry);

            AddOrUpdate(customRegionGroupModel);
        }

        public AirportModel GetAirport(string entry)
        {
            var airportModel = new AirportModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                airportModel = dbSession.Query<AirportModel>().FirstOrDefault(a => a.airport_name.Contains(entry));
            }
            return airportModel;
        }

        public CityModel GetCity(string entry)
        {
            var cityModel = new CityModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                cityModel = dbSession.Query<CityModel>().FirstOrDefault(a => a.city_name.Contains(entry));
            }
            return cityModel;
        }

        public StateModel GetState(string entry)
        {
            var stateModel = new StateModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                stateModel = dbSession.Query<StateModel>().FirstOrDefault(a => a.state_name.Contains(entry));
            }
            return stateModel;
        }

        public CountryModel GetCountry(string entry)
        {
            var countryModel = new CountryModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                countryModel = dbSession.Query<CountryModel>().FirstOrDefault(a => a.country_name.Contains(entry));
            }
            return countryModel;
        }

        public RegionModel GetRegion(string entry)
        {
            var regionModel = new RegionModel();
            using (var dbSession = NHibernateHelper.OpenSession())
            {
                regionModel = dbSession.Query<RegionModel>().FirstOrDefault(a => a.region_name.Contains(entry));
            }
            return regionModel;
        }
    }
}
